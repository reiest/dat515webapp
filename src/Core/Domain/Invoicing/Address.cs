using System;
using Microsoft.EntityFrameworkCore;
using src.SharedKernel;

namespace src.Core.Domain.Invoicing;

[Owned]
public class Address : BaseEntity
{
    public Address(string building, string roomNumber, string notes)
    {
        Building = building;
        RoomNumber = roomNumber;
        Notes = notes;
    }

    public string Building { get; protected set; }
    public string RoomNumber { get; protected set; }
    public string Notes { get; protected set; }
}


public class AddressBuildingValidator : IValidator<Address>
{
    public (bool, string) IsValid(Address Address)
    {
        _ = Address ?? throw new ArgumentNullException(nameof(Address), "Cannot validate null object");
        if (string.IsNullOrWhiteSpace(Address.Building)) return (false, "Building cannot be empty");
        return (true, "");
    }
}

public class AddressRoomNumberValidator : IValidator<Address>
{
    public (bool, string) IsValid(Address Address)
    {
        _ = Address ?? throw new ArgumentNullException(nameof(Address), "Cannot validate null object");
        if (string.IsNullOrWhiteSpace(Address.RoomNumber)) return (false, "Roomnumber cannot be empty");
        return (true, "");
    }
}
