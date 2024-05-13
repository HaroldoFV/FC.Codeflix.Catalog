using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
}