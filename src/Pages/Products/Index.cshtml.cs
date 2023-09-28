using System.Collections.Generic;
using System.Threading.Tasks;
using src.Core.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace src.Pages.Products;

public class IndexModel : PageModel
{
	private readonly IMediator _mediator;

	public IndexModel(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

	public List<FoodItem> FoodItems { get; set; } = new();

	public async Task OnGetAsync()
		=> FoodItems = await _mediator.Send(new Core.Domain.Products.Pipelines.Get.Request());

}
