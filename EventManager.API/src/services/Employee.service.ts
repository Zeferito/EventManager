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

import { EmployeeDto } from '../dto/Employee.dto';
import { Employee } from '../entities/Employee.entity';
import { EntityNotFoundError } from '../errors/EntityNotFoundError';
import { OptimisticLockingFailureError } from '../errors/OptimisticLockingFailureError';

@Injectable()
export class EmployeeService {
    constructor(
        @InjectRepository(Employee, 'mySqlConnection')
        private readonly employeeRepository: Repository<Employee>
    ) {}

    async getAll(): Promise<Employee[]> {
        return await this.employeeRepository.find();
    }

    async getById(id: number): Promise<Employee> {
        const employee = await this.employeeRepository.findOneBy({ id: id });

        if (!employee) {
            throw new EntityNotFoundError('Employee not found');
        }

        return employee;
    }

    async insert(employeeDto: EmployeeDto): Promise<Employee> {
        const employee = new Employee();

        employee.name = employeeDto.name;

        return await this.employeeRepository.save(employee);
    }

    async update(id: number, employeeDto: EmployeeDto): Promise<Employee> {
        const existingEmployee = await this.employeeRepository.findOneBy({
            id: id
        });

        if (!existingEmployee) {
            throw new EntityNotFoundError('Employee not found');
        }

        if (employeeDto.version == null) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingEmployee.version,
                -1
            );
        }

        if (employeeDto.version !== existingEmployee.version) {
            throw new OptimisticLockingFailureError(
                'Resource versions do not match',
                existingEmployee.version,
                employeeDto.version
            );
        }

        existingEmployee.name = employeeDto.name;

        return await this.employeeRepository.save(existingEmployee);
    }

    async delete(id: number): Promise<DeleteResult> {
        return await this.employeeRepository.delete(id);
    }
}
