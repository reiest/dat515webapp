using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using src.Core.Domain.Ordering;
using src.Core.Domain.Ordering.Pipelines;

namespace src.Pages.Orders;

public class OrdersModel : PageModel
{
    private IMediator _mediator;

    public OrdersModel(IMediator mediator) => _mediator = mediator;

    public IEnumerable<Order>? Orders { get; private set; }

    public async Task OnGetAsync()
    {
        Orders = await _mediator.Send(new GetOrders.Request());
    }

}