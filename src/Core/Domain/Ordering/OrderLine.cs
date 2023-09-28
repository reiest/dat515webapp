using src.Core.Domain.Products;

namespace src.Core.Domain.Ordering
{
    public class OrderLine
    {
        public OrderLine(int itemId, string itemName, decimal price, int amount)
        {
            ItemId = itemId;
            ItemName = itemName;
            Price = price;
            Amount = amount;
        }
        public int Id { get; protected set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}