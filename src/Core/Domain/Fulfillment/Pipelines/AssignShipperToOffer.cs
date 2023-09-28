using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Core.Domain.Fulfillment.Events;
using src.Infrastructure.Data;
using src.SharedKernel;

namespace src.Core.Domain.Fulfillment.Pipelines;

public class AssignShipperToOffer
{
    public record Request(int offerId, string shipperName) : IRequest<Response>;

    public record Response(bool Success, int OfferId, string[] Errors);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ShopContext _db;
        private readonly IEnumerable<IValidator<Shipper>> _validators;

        public Handler(ShopContext db, IEnumerable<IValidator<Shipper>> validators)
        {
            _db = db ?? throw new System.ArgumentNullException(nameof(db));
            _validators = validators ?? throw new System.ArgumentNullException(nameof(validators));
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.Include(o => o.Shipper).Include(o => o.Reimbursement).SingleOrDefaultAsync(o => o.Id == request.offerId);
            if (offer == null)
            {
                return new Response(false, 0, new[] { "Offer not found" });
            }
            var shipper = await _db.Shippers.SingleOrDefaultAsync(s => s.Name == request.shipperName);
            if (shipper == null)
            {
                shipper = new Shipper(request.shipperName);
            }

            var errors = _validators.Select(v => v.IsValid(shipper))
                .Where(result => !result.IsValid)
                .Select(result => result.Error)
                .ToArray();
            if (errors.Any())
            {
                return new Response(false, 0, errors);
            }
            offer.Shipper = shipper;
            offer.Reimbursement.Shipper = shipper;
            offer.Events.Add(new ShipperAssignedEvent(offer.OrderId));
            await _db.SaveChangesAsync(cancellationToken);
            return new Response(true, offer.Id, Array.Empty<string>());
        }

    }
}