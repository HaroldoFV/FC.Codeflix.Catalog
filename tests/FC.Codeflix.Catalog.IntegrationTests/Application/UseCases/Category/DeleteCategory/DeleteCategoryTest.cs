using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using ApplicationUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Microsoft.EntityFrameworkCore;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest(DeleteCategoryTestFixture fixture)
{
    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var dbContext = fixture.CreateDbContext();
        var categoryExample = fixture.GetExampleCategory();
        var exampleList = fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleList);
        var tracking = await dbContext.AddAsync(categoryExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.DeleteCategory(
            repository, unitOfWork
        );
        var input = new DeleteCategoryInput(categoryExample.Id);

        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = fixture.CreateDbContext(true);
        var dbCategoryDeleted = await assertDbContext.Categories
            .FindAsync(categoryExample.Id);
        dbCategoryDeleted.Should().BeNull();
        var dbCategories = await assertDbContext
            .Categories.ToListAsync();
        dbCategories.Should().HaveCount(exampleList.Count);
    }

    [Fact(DisplayName = nameof(DeleteCategoryThrowsWhenNotFound))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategoryThrowsWhenNotFound()
    {
        var dbContext = fixture.CreateDbContext();
        var exampleList = fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.DeleteCategory(
            repository, unitOfWork
        );
        var input = new DeleteCategoryInput(Guid.NewGuid());

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }
}