using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Core.Domain.Products.Events;
using src.Infrastructure.Data;

namespace src.Core.Domain.Cart.Handlers;

public class FoodItemPriceChangedHandler : INotificationHandler<FoodItemPriceChanged>
{
    private readonly ShopContext _db;

    public FoodItemPriceChangedHandler(ShopContext db)
        => _db = db ?? throw new System.ArgumentNullException(nameof(db));

    public Task Handle(FoodItemPriceChanged notification, CancellationToken cancellationToken)
    {
        var carts = _db.ShoppingCart.Include(c => c.Items)
                        .Where(c => c.Items.Any(i => i.Sku == notification.ItemId))
                        .ToListAsync(cancellationToken);

        foreach (var cart in carts.Result)
        {
            foreach (var item in cart.Items.Where(i => i.Sku == notification.ItemId))
            {
                item.Price = notification.NewPrice;
            }
        }
        return _db.SaveChangesAsync(cancellationToken);
    }
}