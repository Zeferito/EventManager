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
import { JwtService } from '@nestjs/jwt';

import bcrypt from 'bcrypt';

import { LoginRequestDto } from '../dto/LoginRequest.dto';
import { RegisterUserDto } from '../dto/RegisterUser.dto';
import { User } from '../entities/User.entity';
import { Role } from '../enums/Role.enum';
import { EntityNotFoundError } from '../errors/EntityNotFoundError';
import { PasswordMismatchError } from '../errors/PasswordMismatchError';
import { UserConflictError } from '../errors/UserConflictError';
import { UserService } from './User.service';

@Injectable()
export class AuthService {
    constructor(
        private readonly userService: UserService,
        private readonly jwtService: JwtService
    ) {}

    async register(registerUserDto: RegisterUserDto) {
        const existingUser = await this.userService.getByUsername(
            registerUserDto.username
        );

        if (existingUser) {
            throw new UserConflictError(
                'A user already exists with the same username'
            );
        }

        const hashedPassword = await bcrypt.hash(registerUserDto.password, 10);

        const user = new User();

        user.username = registerUserDto.username;
        user.password = hashedPassword;
        user.roles = [Role.Other];

        return await this.userService.save(user);
    }

    async login(loginRequest: LoginRequestDto): Promise<string> {
        const user = await this.userService.getByUsername(
            loginRequest.identifier
        );

        if (!user) {
            throw new EntityNotFoundError('User not found');
        }

        const passwordMatch = await bcrypt.compare(
            loginRequest.password,
            user.password
        );

        if (!passwordMatch) {
            throw new PasswordMismatchError('Invalid password');
        }

        return this.jwtService.signAsync({
            identifier: loginRequest.identifier
        });
    }
}
