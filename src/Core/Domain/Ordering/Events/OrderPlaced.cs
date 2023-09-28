using src.SharedKernel;

namespace src.Core.Domain.Ordering.Events;

public record OrderPlaced(int orderId, Customer customer, Location location, decimal amount) : BaseDomainEvent;