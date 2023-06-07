namespace Domain.Exceptions;

using Common.Constants;
using Domain.Exceptions.Base;
using System;

public sealed class BikeNotFoundException : NotFoundException
{
    public BikeNotFoundException(Guid bikeId)
        : base(string.Format(GlobalConstants.Bike.NotFoundMessage, bikeId))
    {
    }
}