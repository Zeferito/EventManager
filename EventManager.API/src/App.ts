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
import { ValidationPipe, VersioningType } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { NestFactory } from '@nestjs/core';
import {
    DocumentBuilder,
    SwaggerDocumentOptions,
    SwaggerModule
} from '@nestjs/swagger';

import helmet from 'helmet';

import { HttpExceptionFilter } from './filters/HttpException.filter';
import { AppModule } from './modules/App.module';

async function bootstrap() {
    const app = await NestFactory.create(AppModule);

    const configService = app.get(ConfigService);

    const allowedOrigins =
        configService.get<string>('CLIENT_URL') || 'http://localhost:3001';

    app.enableCors({
        origin: [allowedOrigins],
        methods: ['GET', 'PUT', 'POST', 'DELETE'],
        credentials: true
    });

    app.use(helmet());

    const apiPath = 'eventmanager/api';

    app.setGlobalPrefix(apiPath);

    app.enableVersioning({
        type: VersioningType.URI
    });

    app.useGlobalPipes(new ValidationPipe());
    app.useGlobalFilters(new HttpExceptionFilter());

    const options: SwaggerDocumentOptions = {
        operationIdFactory: (controllerKey: string, methodKey: string) =>
            methodKey
    };

    const config = new DocumentBuilder()
        .setTitle('Event Manager API')
        .setVersion('1.0')
        .addBearerAuth(
            {
                type: 'http',
                scheme: 'bearer',
                bearerFormat: 'JWT',
                name: 'JWT',
                description: 'Enter JWT token',
                in: 'header'
            },
            'JWT-auth'
        )
        .build();

    const document = SwaggerModule.createDocument(app, config, options);

    SwaggerModule.setup(apiPath, app, document);

    const port = configService.get<number>('SERVER_PORT')!;

    return app.listen(port, () => {
        console.log(`Server is running on port ${port}`);
    });
}

(async () => {
    await bootstrap();
})();
