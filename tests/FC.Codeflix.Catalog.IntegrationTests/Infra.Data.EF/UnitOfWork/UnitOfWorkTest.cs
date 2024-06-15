using UnitOfWorkInfra = FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using FC.Codeflix.Catalog.Application;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest(UnitOfWorkTestFixture fixture)
{
    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleCategoriesList = fixture.GetExampleCategoriesList();
        // var categoryWithEvent = exampleCategoriesList.First();
        // var @event = new DomainEventFake();
        // categoryWithEvent.RaiseEvent(@event);
        // var eventHandlerMock = new Mock<IDomainEventHandler<DomainEventFake>>();
        await dbContext.AddRangeAsync(exampleCategoriesList);
        // var serviceCollection = new ServiceCollection();
        // serviceCollection.AddLogging();
        // serviceCollection.AddSingleton(eventHandlerMock.Object);
        // var serviceProvider = serviceCollection.BuildServiceProvider();
        // var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWorkInfra.UnitOfWork>>());

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = fixture.CreateDbContext(true);
        var savedCategories = assertDbContext.Categories
            .AsNoTracking().ToList();
        savedCategories.Should()
            .HaveCount(exampleCategoriesList.Count);
        // eventHandlerMock.Verify(x =>
        // x.HandleAsync(@event, It.IsAny<CancellationToken>()),
        // Times.Once);
        // categoryWithEvent.Events.Should().BeEmpty();
    }


    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = fixture.CreateDbContext();
        // var serviceCollection = new ServiceCollection();
        // serviceCollection.AddLogging();
        // var serviceProvider = serviceCollection.BuildServiceProvider();
        // var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWorkInfra.UnitOfWork>>());

        var task = async ()
            => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}