using System;
using Microsoft.EntityFrameworkCore;
using src.SharedKernel;

namespace src.Core.Domain.Ordering;

[Owned]
public class Location : BaseEntity
{
    public Location(string building, string roomNumber, string notes)
    {
        Building = building;
        RoomNumber = roomNumber;
        Notes = notes;
    }

    public string Building { get; protected set; }
    public string RoomNumber { get; protected set; }
    public string Notes { get; protected set; }
}


public class LocationBuildingValidator : IValidator<Location>
{
    public (bool, string) IsValid(Location location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location), "Cannot validate null object");
        if (string.IsNullOrWhiteSpace(location.Building)) return (false, "Building cannot be empty");
        return (true, "");
    }
}

public class LocationRoomNumberValidator : IValidator<Location>
{
    public (bool, string) IsValid(Location location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location), "Cannot validate null object");
        if (string.IsNullOrWhiteSpace(location.RoomNumber)) return (false, "Roomnumber cannot be empty");
        return (true, "");
    }
}
