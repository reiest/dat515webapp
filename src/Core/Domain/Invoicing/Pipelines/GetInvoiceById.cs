using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Infrastructure.Data;

namespace src.Core.Domain.Invoicing.Pipelines;

public class GetInvoiceById
{
    public record Request(int orderId) : IRequest<Invoice>;

    public class Handler : IRequestHandler<Request, Invoice>
    {
        private readonly ShopContext _db;

        public Handler(ShopContext db)
        {
            _db = db ?? throw new System.ArgumentNullException(nameof(db));
        }

        public async Task<Invoice> Handle(Request request, CancellationToken cancellationToken)
        {
            var invoice = await _db.Invoices
                            .Include(i => i.Customer)
                            .Include(i => i.Address)
                            .SingleOrDefaultAsync(i => i.OrderId == request.orderId);
            return invoice;
        }
    }
}