using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
public class CategoryPersistence(CodeflixCatalogDbContext context)
{
    public async Task<DomainEntity.Category?> GetById(Guid id)
        => await context
            .Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    /*public async Task InsertList(List<DomainEntity.Category> categories)
    {
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }*/
}
