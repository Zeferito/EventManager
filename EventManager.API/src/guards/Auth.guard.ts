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
import { CanActivate, ExecutionContext, Injectable } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { Reflector } from '@nestjs/core';
import { JwtService } from '@nestjs/jwt';

import { Request } from 'express';

import { ROLES_KEY } from '../decorators/Roles.decorator';
import { Role } from '../enums/Role.enum';
import { TokenVerificationError } from '../errors/TokenVerificationError';
import { UserService } from '../services/User.service';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private readonly userService: UserService,
        private readonly reflector: Reflector,
        private readonly jwtService: JwtService,
        private readonly configService: ConfigService
    ) {}

    async canActivate(context: ExecutionContext): Promise<boolean> {
        const requiredRoles = this.reflector.getAllAndOverride<Role[]>(
            ROLES_KEY,
            [context.getHandler(), context.getClass()]
        );

        if (!requiredRoles) {
            return true;
        }

        const req: Request = context.switchToHttp().getRequest();

        const authHeader = req.header('Authorization');

        if (!authHeader) {
            throw new TokenVerificationError(
                'Access denied. No token provided.'
            );
        }

        const token = authHeader.split(' ')[1];

        if (!token) {
            throw new TokenVerificationError(
                'Access denied. No token provided.'
            );
        }

        let decodedToken;

        try {
            decodedToken = (await this.jwtService.verifyAsync(token, {
                secret: this.configService.get<string>('JWT_SECRET')
            })) as {
                identifier: string;
            };
        } catch (error: any) {
            throw new TokenVerificationError(error.message);
        }

        const identifier = decodedToken.identifier;

        const user = await this.userService.getByUsername(identifier);

        if (!user) {
            throw new TokenVerificationError(
                'Access denied. Invalid token provided.'
            );
        }

        return requiredRoles.some((role) => user.roles?.includes(role));
    }
}
