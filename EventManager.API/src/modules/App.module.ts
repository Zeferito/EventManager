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
import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { APP_GUARD } from '@nestjs/core';
import { TypeOrmModule } from '@nestjs/typeorm';

import Joi from 'joi';

import { AuthGuard } from '../guards/Auth.guard';
import { MySqlConfigService } from '../services/MySqlConfig.service';
import { AuthModule } from './Auth.module';
import { ClientModule } from './Client.module';
import { EmployeeModule } from './Employee.module';
import { EventModule } from './Event.module';
import { MaterialModule } from './Material.module';
import { RoomModule } from './Room.module';

@Module({
    imports: [
        ConfigModule.forRoot({
            isGlobal: true,
            cache: true,
            validationSchema: Joi.object({
                NODE_ENV: Joi.string()
                    .valid('development', 'production', 'test', 'provision')
                    .default('development'),
                SERVER_PORT: Joi.number().port().default(3000),
                JWT_SECRET: Joi.string().required(),
                JWT_EXPIRATION: Joi.string().default('1h'),
                MYSQL_HOST: Joi.string().default('localhost'),
                MYSQL_PORT: Joi.number().port().default(3306),
                MYSQL_USER: Joi.string().default('root'),
                MYSQL_PASSWORD: Joi.string().default('password'),
                MYSQL_NAME: Joi.string().default('mydb'),
                MYSQL_SYNCHRONIZE: Joi.boolean().default(true)
            }),
            validationOptions: {
                allowUnknown: true,
                abortEarly: true
            }
        }),
        TypeOrmModule.forRootAsync({
            name: 'mySqlConnection',
            useClass: MySqlConfigService
        }),
        AuthModule,
        EventModule,
        ClientModule,
        EmployeeModule,
        MaterialModule,
        RoomModule
    ],
    providers: [
        {
            provide: APP_GUARD,
            useClass: AuthGuard
        }
    ]
})
export class AppModule {}
