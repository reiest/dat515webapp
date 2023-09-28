using System;
using src.Core.Domain.Ordering;
using src.SharedKernel;

namespace src.Core.Domain.Ordering
{
    public class Customer : BaseEntity
    {
        public int Id { get; protected set; }
        public string Name { get; set; }
    }
}

public class LocationRoomNumberValidator : IValidator<Customer>
{
    public (bool, string) IsValid(Customer customer)
    {
        _ = customer ?? throw new ArgumentNullException(nameof(customer), "Cannot validate null object");
        if (string.IsNullOrWhiteSpace(customer.Name)) return (false, "Name cannot be empty");
        return (true, "");
    }
}