using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Catalog.Categories.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(query.Id, cancellationToken);
        return category is null
            ? throw new NotFoundException($"Category with id '{query.Id}' was not found.")
            : new CategoryDto(category.Id, category.Name);
    }
}