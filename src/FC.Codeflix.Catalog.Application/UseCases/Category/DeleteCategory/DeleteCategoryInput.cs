using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInput(Guid id)
    : IRequest
{
    public Guid Id { get; set; } = id;
}