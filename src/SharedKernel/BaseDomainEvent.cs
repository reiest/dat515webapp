using System;
using MediatR;

namespace src.SharedKernel;

public abstract record BaseDomainEvent : INotification
{
	public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
}
