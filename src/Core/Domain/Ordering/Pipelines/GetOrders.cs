using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Infrastructure.Data;

namespace src.Core.Domain.Ordering.Pipelines;

public class GetOrders
{
    public record Request : IRequest<List<Order>>;

    public class Handler : IRequestHandler<Request, List<Order>>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db) => _db = db ?? throw new System.ArgumentNullException(nameof(db));

        public async Task<List<Order>> Handle(Request request, CancellationToken cancellationToken)
            => await _db.Orders
                .Include(o => o.Customer)
                .ToListAsync(cancellationToken: cancellationToken);
    }
}