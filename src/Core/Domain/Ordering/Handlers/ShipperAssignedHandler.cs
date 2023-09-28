using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using src.Core.Domain.Fulfillment.Events;
using src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace src.Core.Domain.Ordering.Handlers;

public class ShipperAssignedHandler : INotificationHandler<ShipperAssignedEvent>
{
    private readonly ShopContext _db;

    public ShipperAssignedHandler(ShopContext db)
    {
        _db = db;
    }

    public async Task Handle(ShipperAssignedEvent notification, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.SingleOrDefaultAsync(o => o.Id == notification.orderId);
        order.Status = Status.Shipped;
        _db.Orders.Update(order);
        await _db.SaveChangesAsync(cancellationToken);
    }
}