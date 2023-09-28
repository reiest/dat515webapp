using System.Linq;
using System.Threading.Tasks;
using src.Core.Domain.Ordering;
using src.Core.Domain.Ordering.Dto;
using src.Infrastructure.Data;

namespace src.Core.Domain.Ordering.Services;

public interface IOrderingService
{
    Task<int> PlaceOrder(Location location, string customerName, OrderLineDto[] orderLines);
}

public class OrderingService : IOrderingService
{
    private readonly ShopContext _db;
    public OrderingService(ShopContext db)
        => _db = db;

    public async Task<int> PlaceOrder(Location location, string customerName, OrderLineDto[] orderLines)
    {
        var Customer = _db.Customers.SingleOrDefault(c => c.Name == customerName);
        if (Customer == null)
        {
            Customer = new Customer() { Name = customerName };
            _db.Customers.Add(Customer);
        }

        var order = new Order()
        {
            Location = location,
            Customer = Customer,
            Status = Status.New,
            Notes = "Order placed by customer"
        };

        foreach (var line in orderLines)
        {
            order.AddOrderLine(line);
        }
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        order.Status = Status.Placed;
        await _db.SaveChangesAsync();

        return order.Id;
    }
}