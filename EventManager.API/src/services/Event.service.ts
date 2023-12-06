/*
 * MIT License
 *
 * Copyright (c) 2023 Kawtious
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';

import { DeleteResult, LessThan, MoreThan, Not, Repository } from 'typeorm';

import { EventDto } from '../dto/Event.dto';
import { Client } from '../entities/Client.entity';
import { Employee } from '../entities/Employee.entity';
import { Event } from '../entities/Event.entity';
import { EventToMaterial } from '../entities/EventToMaterial.entity';
import { Material } from '../entities/Material.entity';
import { Room } from '../entities/Room.entity';
import { User } from '../entities/User.entity';
import { EventStatus } from '../enums/EventStatus.enum';
import { EntityNotFoundError } from '../errors/EntityNotFoundError';
import { InvalidEventOverlapError } from '../errors/InvalidEventOverlapError';
import { MaterialTotalExceededError } from '../errors/MaterialTotalExceededError';
import { MethodArgumentNotValidError } from '../errors/MethodArgumentNotValidError';
import { OptimisticLockingFailureError } from '../errors/OptimisticLockingFailureError';

@Injectable()
export class EventService {
    constructor(
        @InjectRepository(Event, 'mySqlConnection')
        private readonly eventRepository: Repository<Event>,
        @InjectRepository(User, 'mySqlConnection')
        private readonly userRepository: Repository<User>,
        @InjectRepository(Client, 'mySqlConnection')
        private readonly clientRepository: Repository<Client>,
        @InjectRepository(Employee, 'mySqlConnection')
        private readonly employeeRepository: Repository<Employee>,
        @InjectRepository(Room, 'mySqlConnection')
        private readonly roomRepository: Repository<Room>,
        @InjectRepository(Material, 'mySqlConnection')
        private readonly materialRepository: Repository<Material>,
        @InjectRepository(EventToMaterial, 'mySqlConnection')
        private readonly eventToMaterialRepository: Repository<EventToMaterial>
    ) { }

    async getAll(): Promise<Event[]> {
        return await this.eventRepository.find({
            relations: {
                user: true
            }
        });
    }

    async getById(id: number): Promise<Event> {
        const event = await this.eventRepository.findOne({
            relations: {
                user: true,
                clients: true,
                employees: true,
                rooms: true,
                eventToMaterial: { material: true }
            },
            where: { id: id }
        });

        if (!event) {
            throw new EntityNotFoundError('Event not found');
        }

        return event;
    }

    async insert(eventDto: EventDto): Promise<Event> {
        const existingUser = await this.userRepository.findOneBy({
            id: eventDto.userId
        });

        if (!existingUser) {
            throw new EntityNotFoundError('User not found');
        }

        const event = new Event();

        event.status = eventDto.status;
        event.name = eventDto.name;

        if (eventDto.description != null) {
            event.description = eventDto.description;
        }

        event.startDate = eventDto.startDate;
        event.endDate = eventDto.endDate;
        event.user = existingUser;

        const savedEvent = await this.eventRepository.save(event);

        try {
            await this.saveEventRelations(eventDto, savedEvent);
        } catch (err: any) {
            await this.eventRepository.remove(savedEvent);
            throw err;
        }

        return savedEvent;
    }

    async update(id: number, eventDto: EventDto): Promise<Event> {
        const existingEvent = await this.eventRepository.findOne({
            relations: {
                user: true,
                clients: true,
                employees: true,
                rooms: true,
                eventToMaterial: { material: true }
            },
            where: {
                id: id
            }
        });

        if (!existingEvent) {
            throw new EntityNotFoundError('Event not found');
        }

        if (eventDto.version == null) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingEvent.version,
                -1
            );
        }

        if (eventDto.version !== existingEvent.version) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingEvent.version,
                eventDto.version
            );
        }

        const existingUser = await this.userRepository.findOneBy({
            id: eventDto.userId
        });

        if (!existingUser) {
            throw new EntityNotFoundError('User not found');
        }

        existingEvent.status = eventDto.status;
        existingEvent.name = eventDto.name;

        if (eventDto.description != null) {
            existingEvent.description = eventDto.description;
        }

        existingEvent.startDate = eventDto.startDate;
        existingEvent.endDate = eventDto.endDate;
        existingEvent.user = existingUser;

        const updatedEvent = await this.eventRepository.save(existingEvent);

        await this.updateEventRelations(eventDto, updatedEvent);

        return updatedEvent;
    }

    async softDelete(id: number): Promise<DeleteResult> {
        const existingEvent = await this.eventRepository.findOneBy({
            id: id
        });

        if (!existingEvent) {
            throw new EntityNotFoundError('Event not found');
        }

        existingEvent.status = EventStatus.Inactive;

        await this.eventRepository.save(existingEvent);

        return await this.eventRepository.softDelete(id);
    }

    private async saveEventRelations(eventDto: EventDto, event: Event) {
        let clients: Client[] = [];
        let employees: Employee[] = [];
        let rooms: Room[] = [];
        let eventToMaterials: EventToMaterial[] = [];

        if (eventDto.clientIds) {
            for (const clientId of eventDto.clientIds) {
                const existingClient = await this.clientRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: clientId
                    }
                });

                if (!existingClient) {
                    throw new EntityNotFoundError(
                        'Client not found with Id: ' + clientId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        clients: { id: clientId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot register Client on Event due to the client already being' +
                        'assigned to a different Event during the same period',
                        clientId,
                        overlappingEvents
                    );
                }

                existingClient.events.push(event);

                clients.push(existingClient);
            }
        }

        if (eventDto.employeeIds) {
            for (const employeeId of eventDto.employeeIds) {
                const existingEmployee = await this.employeeRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: employeeId
                    }
                });

                if (!existingEmployee) {
                    throw new EntityNotFoundError(
                        'Employee not found with Id: ' + employeeId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        employees: { id: employeeId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot register employee on Event due to the employee already being' +
                        'assigned to a different Event during the same period',
                        employeeId,
                        overlappingEvents
                    );
                }

                existingEmployee.events.push(event);

                employees.push(existingEmployee);
            }
        }

        if (eventDto.roomIds) {
            for (const roomId of eventDto.roomIds) {
                const existingRoom = await this.roomRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: roomId
                    }
                });

                if (!existingRoom) {
                    throw new EntityNotFoundError(
                        'Room not found with Id: ' + roomId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        rooms: { id: roomId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot reserve Room due to it already being reserved for that period',
                        roomId,
                        overlappingEvents
                    );
                }

                existingRoom.events.push(event);

                rooms.push(existingRoom);
            }
        }

        if (eventDto.materialReserved) {
            const valueArr = eventDto.materialReserved.map(function (item) {
                return item.materialId;
            });

            const hasDuplicate = valueArr.some(function (item, idx) {
                return valueArr.indexOf(item) != idx;
            });

            if (hasDuplicate) {
                throw new MethodArgumentNotValidError(
                    'There cannot be duplicate Materials in the request'
                );
            }

            for (const materialReserved of eventDto.materialReserved) {
                const existingMaterial =
                    await this.materialRepository.findOneBy({
                        id: materialReserved.materialId
                    });

                if (!existingMaterial) {
                    throw new EntityNotFoundError(
                        'Material not found with Id: ' +
                        materialReserved.materialId
                    );
                }

                const overlappingEventToMaterials =
                    await this.eventToMaterialRepository.find({
                        relations: { event: true, material: true },
                        where: {
                            event: {
                                status: Not(EventStatus.Inactive),
                                startDate: LessThan(eventDto.endDate),
                                endDate: MoreThan(eventDto.startDate)
                            },
                            material: {
                                id: existingMaterial.id
                            }
                        }
                    });

                let sum = materialReserved.amount;

                for (const eventToMaterial of overlappingEventToMaterials) {
                    sum += eventToMaterial.amountReserved;
                }

                if (sum > existingMaterial.total) {
                    throw new MaterialTotalExceededError(
                        'Cannot reserve material because the sum of its reservations will exceed total during that period',
                        existingMaterial.id,
                        existingMaterial.total,
                        sum,
                        overlappingEventToMaterials
                    );
                }

                const eventToMaterial = new EventToMaterial();

                eventToMaterial.event = event;
                eventToMaterial.material = existingMaterial;
                eventToMaterial.amountReserved = materialReserved.amount;

                eventToMaterials.push(eventToMaterial);
            }
        }

        for (const client of clients) {
            await this.clientRepository.save(client);
        }

        for (const employee of employees) {
            await this.employeeRepository.save(employee);
        }

        for (const room of rooms) {
            await this.roomRepository.save(room);
        }

        for (const eventToMaterial of eventToMaterials) {
            await this.eventToMaterialRepository.save(eventToMaterial);
        }
    }

    private async updateEventRelations(eventDto: EventDto, event: Event) {
        let clients: Client[] = [];
        let employees: Employee[] = [];
        let rooms: Room[] = [];
        let eventToMaterials: EventToMaterial[] = [];

        if (eventDto.clientIds) {
            for (const clientId of eventDto.clientIds) {
                const existingClient = await this.clientRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: clientId
                    }
                });

                if (!existingClient) {
                    throw new EntityNotFoundError(
                        'Client not found with Id: ' + clientId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        id: Not(event.id),
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        clients: { id: clientId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot register Client on Event due to the client already being' +
                        'assigned to a different Event during the same period',
                        clientId,
                        overlappingEvents
                    );
                }

                existingClient.events.push(event);

                clients.push(existingClient);
            }
        }

        if (eventDto.employeeIds) {
            for (const employeeId of eventDto.employeeIds) {
                const existingEmployee = await this.employeeRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: employeeId
                    }
                });

                if (!existingEmployee) {
                    throw new EntityNotFoundError(
                        'Employee not found with Id: ' + employeeId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        id: Not(event.id),
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        employees: { id: employeeId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot register employee on Event due to the employee already being' +
                        'assigned to a different Event during the same period',
                        employeeId,
                        overlappingEvents
                    );
                }

                existingEmployee.events.push(event);

                employees.push(existingEmployee);
            }
        }

        if (eventDto.roomIds) {
            for (const roomId of eventDto.roomIds) {
                const existingRoom = await this.roomRepository.findOne({
                    relations: { events: true },
                    where: {
                        id: roomId
                    }
                });

                if (!existingRoom) {
                    throw new EntityNotFoundError(
                        'Room not found with Id: ' + roomId
                    );
                }

                const overlappingEvents = await this.eventRepository.find({
                    relations: { rooms: true },
                    where: {
                        id: Not(event.id),
                        status: Not(EventStatus.Inactive),
                        startDate: LessThan(eventDto.endDate),
                        endDate: MoreThan(eventDto.startDate),
                        rooms: { id: roomId }
                    }
                });

                if (overlappingEvents.length > 0) {
                    throw new InvalidEventOverlapError(
                        'Cannot reserve Room due to it already being reserved for that period',
                        roomId,
                        overlappingEvents
                    );
                }

                existingRoom.events.push(event);

                rooms.push(existingRoom);
            }
        }

        if (eventDto.materialReserved) {
            const valueArr = eventDto.materialReserved.map(function (item) {
                return item.materialId;
            });

            const hasDuplicate = valueArr.some(function (item, idx) {
                return valueArr.indexOf(item) != idx;
            });

            if (hasDuplicate) {
                throw new MethodArgumentNotValidError(
                    'There cannot be duplicate Materials in the request'
                );
            }

            for (const materialReserved of eventDto.materialReserved) {
                const existingMaterial =
                    await this.materialRepository.findOneBy({
                        id: materialReserved.materialId
                    });

                if (!existingMaterial) {
                    throw new EntityNotFoundError(
                        'Material not found with Id: ' +
                        materialReserved.materialId
                    );
                }

                const overlappingEventToMaterials =
                    await this.eventToMaterialRepository.find({
                        relations: { event: true, material: true },
                        where: {
                            event: {
                                id: Not(event.id),
                                status: Not(EventStatus.Inactive),
                                startDate: LessThan(eventDto.endDate),
                                endDate: MoreThan(eventDto.startDate)
                            },
                            material: {
                                id: existingMaterial.id
                            }
                        }
                    });

                let sum = materialReserved.amount;

                for (const eventToMaterial of overlappingEventToMaterials) {
                    sum += eventToMaterial.amountReserved;
                }

                if (sum > existingMaterial.total) {
                    throw new MaterialTotalExceededError(
                        'Cannot reserve material because the sum of its reservations will exceed total during that period',
                        existingMaterial.id,
                        existingMaterial.total,
                        sum,
                        overlappingEventToMaterials
                    );
                }

                const eventToMaterial = new EventToMaterial();

                eventToMaterial.event = event;
                eventToMaterial.material = existingMaterial;
                eventToMaterial.amountReserved = materialReserved.amount;

                eventToMaterials.push(eventToMaterial);
            }
        }

        for (const clientToRemove of event.clients) {
            event.clients = event.clients.filter((client) => {
                return client.id !== clientToRemove.id;
            });
        }

        for (const employeeToRemove of event.employees) {
            event.employees = event.employees.filter((employee) => {
                return employee.id !== employeeToRemove.id;
            });
        }

        for (const roomToRemove of event.rooms) {
            event.rooms = event.rooms.filter((room) => {
                return room.id !== roomToRemove.id;
            });
        }

        const eventToMaterials1: EventToMaterial[] =
            await this.eventToMaterialRepository.find({
                where: {
                    eventId: event.id
                }
            });

        await this.eventToMaterialRepository.remove(eventToMaterials1);

        await this.eventRepository.save(event);

        for (const client of clients) {
            try {
                await this.clientRepository.save(client);
            } catch (error) {}
        }
        
        for (const employee of employees) {
            try {
                await this.employeeRepository.save(employee);
            } catch (error) {}
        }
        
        for (const room of rooms) {
            try {
                await this.roomRepository.save(room);
            } catch (error) {}
        }
        
        for (const eventToMaterial of eventToMaterials) {
            try {
                await this.eventToMaterialRepository.save(eventToMaterial);
            } catch (error) {}
        }
    }
}
