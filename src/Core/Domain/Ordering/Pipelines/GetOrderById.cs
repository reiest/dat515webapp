using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Infrastructure.Data;

namespace src.Core.Domain.Ordering.Pipelines;

public class GetOrderById
{
    public record Request(int OrderId) : IRequest<Order?>;

    public class Handler : IRequestHandler<Request, Order?>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db) => _db = db ?? throw new System.ArgumentNullException(nameof(db));

        public async Task<Order?> Handle(Request request, CancellationToken cancellationToken)
            => await _db.Orders.Include(o => o.OrderLines)
                .Include(o => o.Customer)
                .SingleOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
    }
}