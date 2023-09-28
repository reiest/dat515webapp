using System.Threading.Tasks;
using src.Core.Domain.Products;
using src.Core.Domain.Products.Pipelines;
using src.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace src.Pages.Products;

public class DeleteModel : PageModel
{
	private readonly IMediator _mediator;

	public DeleteModel(IMediator mediator)
	{
		_mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
	}

	public FoodItem? Item { get; private set; }

	public async Task<IActionResult> OnGetAsync(int id)
	{
		try
		{
			Item = await _mediator.Send(new GetById.Request(id));
			return Page();
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
	}

	public async Task<IActionResult> OnPostAsync(int id)
	{
		await _mediator.Send(new Delete.Request(id));

		return RedirectToPage("Index");

	}
}
