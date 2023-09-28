using System.Threading;
using System.Threading.Tasks;
using MediatR;
using src.Core.Domain.Ordering.Events;
using src.Infrastructure.Data;

namespace src.Core.Domain.Invoicing.Handlers;

public class OrderPlacedHandler : INotificationHandler<OrderPlaced>
{
    private readonly ShopContext _db;

    public OrderPlacedHandler(ShopContext db)
    {
        _db = db;
    }

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        var invoice = new Invoice(notification.orderId);
        var customer = new Customer(notification.customer.Name);
        var Address = new Address(notification.location.Building, notification.location.RoomNumber, notification.location.Notes);
        invoice.Customer = customer;
        invoice.Address = Address;
        invoice.Amount = notification.amount;
        invoice.Status = Status.New;
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(cancellationToken);
    }
}