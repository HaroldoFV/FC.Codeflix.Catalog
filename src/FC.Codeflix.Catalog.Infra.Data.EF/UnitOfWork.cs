using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.SeedWork;

namespace FC.Codeflix.Catalog.Infra.Data.EF;

public class UnitOfWork(
        CodeflixCatalogDbContext context)
    // IDomainEventPublisher publisher,
    // ILogger<UnitOfWork> logger)
    : IUnitOfWork
{
    public async Task Commit(CancellationToken cancellationToken)
    {
        // var aggregateRoots = context.ChangeTracker
        //     .Entries<AggregateRoot>()
        //     // .Where(entry => entry.Entity.Events.Any())
        //     .Select(entry => entry.Entity);
        //
        // var enumerable = aggregateRoots as AggregateRoot[] ?? aggregateRoots.ToArray();
        // logger.LogInformation(
        // "Commit: {AggregatesCount} aggregate roots with events.",
        // enumerable.Count());

        // var events = enumerable
        //     .SelectMany(aggregate => aggregate.Events);

        // var domainEvents = events as DomainEvent[] ?? events.ToArray();
        // logger.LogInformation(
        // "Commit: {EventsCount} events raised.", domainEvents.Count());

        // foreach (var @event in domainEvents)
        // await publisher.PublishAsync((dynamic)@event, cancellationToken);

        // foreach (var aggregate in enumerable)
        //     aggregate.ClearEvents();

        await context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}