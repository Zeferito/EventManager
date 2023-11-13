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
import {
    BeforeRemove,
    BeforeSoftRemove,
    Column,
    CreateDateColumn,
    DeleteDateColumn,
    Entity,
    JoinColumn,
    JoinTable,
    ManyToMany,
    ManyToOne,
    OneToMany,
    PrimaryGeneratedColumn,
    Relation,
    UpdateDateColumn,
    VersionColumn
} from 'typeorm';

import { EventStatus } from '../enums/EventStatus.enum';
import { Client } from './Client.entity';
import { Employee } from './Employee.entity';
import { EventToMaterial } from './EventToMaterial.entity';
import { Room } from './Room.entity';
import { User } from './User.entity';

@Entity()
export class Event {
    @PrimaryGeneratedColumn()
    id!: number;

    @CreateDateColumn()
    createdDate!: Date;

    @UpdateDateColumn()
    updatedDate!: Date;

    @DeleteDateColumn()
    deletedDate!: Date;

    @VersionColumn()
    version!: number;

    @Column({
        type: 'text',
        nullable: false
    })
    status!: EventStatus;

    @Column({
        nullable: false
    })
    name!: string;

    @Column({
        nullable: true
    })
    description!: string;

    @Column({
        nullable: false
    })
    startDate!: Date;

    @Column({
        nullable: false
    })
    endDate!: Date;

    @ManyToOne(() => User, (user) => user.events, {
        nullable: false
    })
    @JoinColumn()
    user!: Relation<User>;

    @ManyToMany(() => Client, (client) => client.events)
    @JoinTable({ name: 'event_client' })
    clients!: Client[];

    @ManyToMany(() => Employee, (employee) => employee.events)
    @JoinTable({ name: 'event_employee' })
    employees!: Employee[];

    @ManyToMany(() => Room, (room) => room.events)
    @JoinTable({ name: 'event_room' })
    rooms!: Room[];

    @OneToMany(
        () => EventToMaterial,
        (eventToMaterial) => eventToMaterial.event
    )
    eventToMaterial!: EventToMaterial[];

    @BeforeSoftRemove()
    @BeforeRemove()
    updateStatus() {
        this.status = EventStatus.Inactive;
    }
}
