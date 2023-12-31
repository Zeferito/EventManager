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

import { DeleteResult, Repository } from 'typeorm';

import { ClientDto } from '../dto/Client.dto';
import { EmployeeDto } from '../dto/Employee.dto';
import { Client } from '../entities/Client.entity';
import { Employee } from '../entities/Employee.entity';
import { EntityNotFoundError } from '../errors/EntityNotFoundError';
import { OptimisticLockingFailureError } from '../errors/OptimisticLockingFailureError';

@Injectable()
export class ClientService {
    constructor(
        @InjectRepository(Client, 'mySqlConnection')
        private readonly clientRepository: Repository<Client>
    ) {}

    async getAll(): Promise<Client[]> {
        return await this.clientRepository.find();
    }

    async getById(id: number): Promise<Client> {
        const client = await this.clientRepository.findOneBy({ id: id });

        if (!client) {
            throw new EntityNotFoundError('Client not found');
        }

        return client;
    }

    async insert(clientDto: ClientDto): Promise<Client> {
        const client = new Client();

        client.name = clientDto.name;

        if (clientDto.phone != null) {
            client.phone = clientDto.phone;
        }

        return await this.clientRepository.save(client);
    }

    async update(id: number, clientDto: ClientDto): Promise<Employee> {
        const existingClient = await this.clientRepository.findOneBy({
            id: id
        });

        if (!existingClient) {
            throw new EntityNotFoundError('Client not found');
        }

        if (clientDto.version == null) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingClient.version,
                -1
            );
        }

        if (clientDto.version !== existingClient.version) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingClient.version,
                clientDto.version
            );
        }

        existingClient.name = clientDto.name;

        if (clientDto.phone != null) {
            existingClient.phone = clientDto.phone;
        }

        return await this.clientRepository.save(existingClient);
    }

    async delete(id: number): Promise<DeleteResult> {
        return await this.clientRepository.delete(id);
    }
}
