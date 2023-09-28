using System.Threading.Tasks;

namespace src.Core.Domain.Ordering.Dto;

public record OrderLineDto
(
    int FoodItemId,
    string FoodItemName,
    int Amount,
    decimal Price
);