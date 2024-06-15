using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using ApplicationUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Application;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest(UpdateCategoryTestFixture fixture)
{
    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategory(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput input
    )
    {
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetExampleCategoriesList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await fixture.CreateDbContext(true)
            .Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryWithoutIsActive(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description
        );
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetExampleCategoriesList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await fixture.CreateDbContext(true)
            .Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryOnlyName(
        DomainEntity.Category exampleCategory,
        UpdateCategoryInput exampleInput
    )
    {
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name
        );
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetExampleCategoriesList());
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (fixture.CreateDbContext(true))
            .Categories.FindAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    public async Task UpdateThrowsWhenNotFoundCategory()
    {
        var input = fixture.GetValidInput();
        var dbContext = fixture.CreateDbContext();
        await dbContext.AddRangeAsync(fixture.GetExampleCategoriesList());
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }

    [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 6,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateThrowsWhenCantInstantiateCategory(
        UpdateCategoryInput input,
        string expectedExceptionMessage
    )
    {
        var dbContext = fixture.CreateDbContext();
        var exampleCategories = fixture.GetExampleCategoriesList();
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(dbContext);
        // eventPublisher,
        // serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);
        input.Id = exampleCategories[0].Id;

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
    }
}