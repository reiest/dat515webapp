using System;
using System.Collections.Generic;
using System.Linq;
using src.Core.Domain.Ordering.Dto;
using src.Core.Domain.Ordering.Events;
using src.SharedKernel;

namespace src.Core.Domain.Ordering;

public class Order : BaseEntity
{
    public int Id { get; protected set; }
    public DateTime OrderDate { get; protected set; }
    private readonly List<OrderLine> _orderLines = new();
    public IEnumerable<OrderLine> OrderLines => _orderLines.AsReadOnly();
    public Location Location { get; set; } = null!;
    public string Notes { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    private Status _status;
    public Status Status
    {
        get => _status;
        set
        {
            if (_status != value && value == Status.Placed)
            {
                Events.Add(new OrderPlaced(Id, Customer, Location, CalculateTotalPrice()));
            }
            _status = value;
        }
    }

    public void AddOrderLine(OrderLineDto orderLineDto)
    {
        var orderLine = new OrderLine(orderLineDto.FoodItemId, orderLineDto.FoodItemName, orderLineDto.Price, orderLineDto.Amount);
        _orderLines.Add(orderLine);
    }

    public decimal CalculateTotalPrice()
    {
        return _orderLines.Sum(ol => ol.Price * ol.Amount);
    }
}

