// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;

namespace PetFamily.Domain.Volunteers;

public class Position : ComparableValueObject
{
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Position, Error> Create(int number)
    {
        if (number < 1)
        {
            return Errors.General.ValueIsInvalid("position");
        }

        return new Position(number);
    }

    public Result<Position, Error> Forward()
    {
        return Create(Value + 1);
    }

    public Result<Position, Error> Back()
    {
        return Create(Value - 1);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(Position position)
    {
        return position.Value;
    }
}
