using System.Threading;
using System.Threading.Tasks;
using MediatR;
using src.Core.Domain.Ordering.Events;
using src.Infrastructure.Data;

namespace src.Core.Domain.Fulfillment.Handlers;

public class OrderPlacedHandler : INotificationHandler<OrderPlaced>
{
    private readonly ShopContext _db;

    public OrderPlacedHandler(ShopContext db)
        => _db = db ?? throw new System.ArgumentNullException(nameof(db));

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        var offer = new Offer
        {
            OrderId = notification.orderId,
        };

        var reimbursement = new Reimbursement(notification.amount);
        offer.Reimbursement = reimbursement;
        _db.Offers.Add(offer);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
