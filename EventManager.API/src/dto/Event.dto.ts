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
import { Type } from 'class-transformer';
import {
    IsDate,
    IsInt,
    IsNotEmpty,
    IsNumber,
    IsOptional
} from 'class-validator';

import { IsDateAfter } from '../decorators/IsDateAfter.decorator';
import { IsDateBefore } from '../decorators/IsDateBefore.decorator';
import { IsDateDistant } from '../decorators/IsDateDistant.decorator';
import { NoDuplicates } from '../decorators/NoDuplicates.decorator';
import { EventStatus } from '../enums/EventStatus.enum';

export class EventDto {
    @IsOptional()
    @IsNumber()
    version?: number;

    status!: EventStatus;

    @IsNotEmpty()
    name!: string;

    @IsOptional()
    description?: string;

    @IsDate()
    @IsDateBefore('endDate')
    @IsDateDistant('endDate', 86400)
    @Type(() => Date)
    startDate!: Date;

    @IsDate()
    @IsDateAfter('startDate')
    @Type(() => Date)
    endDate!: Date;

    @IsInt()
    userId!: number;

    @IsOptional()
    @NoDuplicates()
    clientIds?: number[];

    @IsOptional()
    @NoDuplicates()
    employeeIds?: number[];

    @IsOptional()
    @NoDuplicates()
    roomIds?: number[];

    materialReserved?: MaterialReserved[];
}

export class MaterialReserved {
    materialId!: number;

    amount!: number;
}
