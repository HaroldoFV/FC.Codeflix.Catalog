using FC.Codeflix.Catalog.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Application;

public class DomainEventPublisher(IServiceProvider serviceProvider)
    : IDomainEventPublisher
{
    public async Task PublishAsync<TDomainEvent>(
        TDomainEvent domainEvent, CancellationToken cancellationToken)
        where TDomainEvent : DomainEvent
    {
        var handlers = serviceProvider
            .GetServices<IDomainEventHandler<TDomainEvent>>();
        var domainEventHandlers = handlers.ToList();
        if (!domainEventHandlers.Any()) return;
        foreach (var handler in domainEventHandlers)
            await handler.HandleAsync(domainEvent, cancellationToken);
    }
}