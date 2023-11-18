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
    Body,
    Controller,
    Delete,
    Get,
    HttpCode,
    HttpStatus,
    Param,
    Post,
    Put
} from '@nestjs/common';

import { Roles } from '../decorators/Roles.decorator';
import { ClientDto } from '../dto/Client.dto';
import { Role } from '../enums/Role.enum';
import { MethodArgumentNotValidError } from '../errors/MethodArgumentNotValidError';
import { ClientService } from '../services/Client.service';

@Controller({ path: 'clients', version: '1' })
export class ClientController {
    constructor(private readonly clientService: ClientService) {}

    @Get()
    @HttpCode(HttpStatus.OK)
    @Roles(Role.Admin)
    async getAll() {
        return await this.clientService.getAll();
    }

    @Get(':id')
    @HttpCode(HttpStatus.OK)
    @Roles(Role.Admin)
    async getById(
        @Param('id')
        id: string
    ) {
        if (isNaN(Number(id))) {
            throw new MethodArgumentNotValidError('Invalid ID');
        }

        return await this.clientService.getById(Number(id));
    }

    @Post()
    @HttpCode(HttpStatus.CREATED)
    @Roles(Role.Admin)
    async insert(@Body() clientDto: ClientDto) {
        return await this.clientService.insert(clientDto);
    }

    @Put(':id')
    @HttpCode(HttpStatus.OK)
    @Roles(Role.Admin)
    async update(
        @Param('id')
        id: string,
        @Body() clientDto: ClientDto
    ) {
        if (isNaN(Number(id))) {
            throw new MethodArgumentNotValidError('Invalid ID');
        }

        return await this.clientService.update(Number(id), clientDto);
    }

    @Delete(':id')
    @HttpCode(HttpStatus.NO_CONTENT)
    @Roles(Role.Admin)
    async delete(
        @Param('id')
        id: string
    ) {
        if (isNaN(Number(id))) {
            throw new MethodArgumentNotValidError('Invalid ID');
        }

        await this.clientService.delete(Number(id));

        return;
    }
}
