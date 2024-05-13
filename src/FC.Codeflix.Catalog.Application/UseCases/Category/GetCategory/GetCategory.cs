using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory(ICategoryRepository categoryRepository)
    : IGetCategory
{
    public async Task<CategoryModelOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await categoryRepository.Get(request.Id, cancellationToken);
        return CategoryModelOutput.FromCategory(category);
    }
}