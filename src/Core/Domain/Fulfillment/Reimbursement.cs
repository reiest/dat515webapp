using src.SharedKernel;

namespace src.Core.Domain.Fulfillment;

public class Reimbursement : BaseEntity
{
    public Reimbursement(decimal amount)
    {
        Amount = amount;
    }
    public int Id { get; protected set; }
    public Shipper? Shipper { get; set; }
    public decimal Amount { get; set; }
    public int InvoiceId { get; set; }
}