namespace FC.Codeflix.Catalog.Domain.SeedWork;

public abstract class AggregateRoot : Entity
{
    // private readonly List<DomainEvent> _events = new();
    // public IReadOnlyList<DomainEvent> Events => _events.AsReadOnly();

    protected AggregateRoot() : base()
    {
    }

    // public void RaiseEvent(DomainEvent @event) => _events.Add(@event);
    // public void ClearEvents() => _events.Clear();
}