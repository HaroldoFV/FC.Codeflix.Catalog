using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategory(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
)
    : IDeleteCategory
{
    public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.Get(request.Id, cancellationToken);
        await categoryRepository.Delete(category, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }
}