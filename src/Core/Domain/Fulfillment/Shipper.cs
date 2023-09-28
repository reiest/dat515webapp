using System;
using src.SharedKernel;

namespace src.Core.Domain.Fulfillment
{
    public class Shipper : BaseEntity
    {
        public Shipper(string name)
        {
            Name = name;
        }
        public int Id { get; protected set; }
        public string Name { get; set; }
    }

    public class ShipperValidator : IValidator<Shipper>
    {
        public (bool IsValid, string Error) IsValid(Shipper shipper)
        {
            _ = shipper ?? throw new ArgumentNullException(nameof(shipper), "Cannot validate a null object");
            if (string.IsNullOrWhiteSpace(shipper.Name))
            {
                return (false, "Name cannot be empty");
            }
            return (true, string.Empty);
        }
    }
}

