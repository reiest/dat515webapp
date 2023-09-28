using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Infrastructure.Data;
using src.SharedKernel;

namespace src.Core.Domain.Fulfillment.Pipelines;

public class GetOfferById
{
    public record Request(int orderId) : IRequest<Offer>;

    public class Handler : IRequestHandler<Request, Offer>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db)
        {
            _db = db ?? throw new System.ArgumentNullException(nameof(db));
        }

        public async Task<Offer> Handle(Request request, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.Include(o => o.Shipper).Include(o => o.Reimbursement).SingleOrDefaultAsync(o => o.OrderId == request.orderId);
            return offer;
        }
    }
}