using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using src.Core.Domain.Fulfillment;
using src.Core.Domain.Fulfillment.Pipelines;
using src.Core.Domain.Invoicing;
using src.Core.Domain.Invoicing.Pipelines;
using src.Core.Domain.Ordering;
using src.Core.Domain.Ordering.Pipelines;

namespace src.Pages.Orders;

public class OrderDetailsModel : PageModel
{
    private readonly IMediator _mediator;

    public OrderDetailsModel(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

    public Order? Order { get; private set; }
    public Offer? Offer { get; private set; }
    public Invoice? Invoice { get; private set; }
    [BindProperty]
    public string ShipperName { get; set; } = string.Empty;
    public string[] Errors { get; private set; } = System.Array.Empty<string>();

    public async Task OnGetAsync(int id)
    {
        var orderId = id;

        Order = await _mediator.Send(new GetOrderById.Request(orderId));
        Offer = await _mediator.Send(new GetOfferById.Request(orderId));
        Invoice = await _mediator.Send(new GetInvoiceById.Request(orderId));
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var orderId = id;
        Order = await _mediator.Send(new GetOrderById.Request(orderId));
        Offer = await _mediator.Send(new GetOfferById.Request(orderId));
        Invoice = await _mediator.Send(new GetInvoiceById.Request(orderId));

        var result = await _mediator.Send(new AssignShipperToOffer.Request(Offer.Id, ShipperName));

        if (result.Success)
        {
            return Page();
        }
        Errors = result.Errors;
        return Page();
    }
}