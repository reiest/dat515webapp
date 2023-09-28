using src.SharedKernel;

namespace src.Core.Domain.Fulfillment.Events;

public record ShipperAssignedEvent(int orderId) : BaseDomainEvent;