﻿namespace FC.Codeflix.Catalog.Domain.SeedWork;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : DomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
}