using src.Core.Domain.Fulfillment.Events;
using src.SharedKernel;

namespace src.Core.Domain.Fulfillment;

public class Offer : BaseEntity
{

    public int Id { get; protected set; }
    public int OrderId { get; set; }
    private Shipper? _shipper;
    public Shipper? Shipper
    {
        get => _shipper;
        set
        {
            _shipper = value;
            Events.Add(new ShipperAssignedEvent(OrderId));
        }
    }
    public Reimbursement? Reimbursement { get; set; }
}