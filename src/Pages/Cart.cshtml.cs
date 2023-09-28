using System.Threading.Tasks;
using src.Core.Domain.Cart;
using src.Core.Domain.Cart.Pipelines;
using src.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace src.Pages;

public class CartModel : PageModel
{
    private readonly IMediator _mediator;
    public string[] Errors { get; private set; } = System.Array.Empty<string>();
    public CartModel(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

    [BindProperty]
    public string customerName { get; set; } = null!;
    [BindProperty]
    public string building { get; set; } = null!;
    [BindProperty]
    public string roomNumber { get; set; } = null!;
    [BindProperty]
    public string notes { get; set; } = null!;

    public ShoppingCart? Cart { get; private set; }

    public async Task OnGetAsync()
    {
        var cartId = HttpContext.Session.GetGuid("CartId");
        if (cartId is null) return;

        Cart = await _mediator.Send(new Get.Request(cartId.Value));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var cartId = HttpContext.Session.GetGuid("CartId");
        if (cartId is null) return RedirectToPage("/Index");
        Cart = await _mediator.Send(new Get.Request(cartId.Value));

        var result = await _mediator.Send(new CartCheckout.Request(cartId.Value, customerName, building, roomNumber, notes));
        if (result.Success)
        {
            HttpContext.Session.Remove("CartId");
            return RedirectToPage("/Orders/OrderDetails", new { id = result.OrderId });
        }
        Errors = result.Errors;
        return Page();
    }
}
