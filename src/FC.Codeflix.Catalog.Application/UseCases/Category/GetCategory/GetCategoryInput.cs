using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInput(Guid id)
    : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; } = id;
}