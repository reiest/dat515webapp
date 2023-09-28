using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Core.Domain.Ordering;
using src.Core.Domain.Ordering.Dto;
using src.Core.Domain.Ordering.Services;
using src.Infrastructure.Data;
using src.SharedKernel;

namespace src.Core.Domain.Cart.Pipelines;

public class CartCheckout
{
    public record Request(Guid CartId, string CustomerName, string Building, string roomNumber, string Notes) : IRequest<Response>;

    public record Response(bool Success, int OrderId, string[] Errors);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IOrderingService _orderingService;
        private readonly ShopContext _db;
        private readonly IEnumerable<IValidator<Location>> _validators;

        public Handler(IOrderingService orderingService, ShopContext db, IEnumerable<IValidator<Location>> validators)
        {
            _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var cart = _db.ShoppingCart.Include(c => c.Items).SingleOrDefault(c => c.Id == request.CartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found");
            }
            var orderLines = cart.Items.Select(i => new OrderLineDto(i.Sku, i.Name, i.Count, i.Price)).ToArray();
            var notes = request.Notes;
            if (request.Notes is null)
            {
                notes = "N/A";
            }
            var location = new Location(request.Building, request.roomNumber, notes);
            var errors = _validators.Select(v => v.IsValid(location))
                .Where(result => !result.IsValid)
                .Select(result => result.Error)
                .ToArray();

            if (string.IsNullOrWhiteSpace(request.CustomerName))
            {
                errors = errors.Append("Customer name cannot be empty").ToArray();
            }

            if (errors.Any())
            {
                return new Response(false, 0, errors);
            }
            var orderId = await _orderingService.PlaceOrder(location, request.CustomerName, orderLines);
            return new Response(true, orderId, Array.Empty<string>());
        }
    }
}