using src.SharedKernel;

namespace src.Core.Domain.Invoicing;

public class Invoice : BaseEntity
{
    public Invoice(int orderId)
    {
        OrderId = orderId;
    }
    public int Id { get; protected set; }
    public Customer? Customer { get; set; }
    public Address? Address { get; set; }
    public decimal Amount { get; set; }
    public int OrderId { get; set; }
    public Status Status { get; set; }
    public Payment? Payment { get; set; }
}