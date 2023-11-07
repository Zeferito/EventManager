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
    ValidationArguments,
    ValidationOptions,
    ValidatorConstraint,
    ValidatorConstraintInterface,
    registerDecorator
} from 'class-validator';

@ValidatorConstraint({ async: true })
export class IsDateDistantConstraint implements ValidatorConstraintInterface {
    validate(value: Date, args: ValidationArguments) {
        const otherDate = (args.object as Record<string, unknown>)[
            args.constraints[0]
        ] as Date;

        const minimumDifferenceSeconds = args.constraints[1] as number;

        if (minimumDifferenceSeconds <= 0) {
            return true;
        }

        const differenceSeconds = Math.abs(
            (otherDate.getTime() - value.getTime()) / 1000
        );

        return differenceSeconds >= minimumDifferenceSeconds;
    }

    defaultMessage(args: ValidationArguments) {
        return `${args.property} and ${args.constraints[0]} must be at least ${args.constraints[1]} seconds apart`;
    }
}

export function IsDateDistant(
    property: string,
    minimumDifferenceSeconds: number,
    validationOptions?: ValidationOptions
) {
    return function (object: Object, propertyName: string) {
        registerDecorator({
            target: object.constructor,
            propertyName: propertyName,
            options: validationOptions,
            constraints: [property, minimumDifferenceSeconds],
            validator: IsDateDistantConstraint
        });
    };
}
