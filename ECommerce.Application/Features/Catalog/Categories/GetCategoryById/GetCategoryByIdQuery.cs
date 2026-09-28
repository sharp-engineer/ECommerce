using MediatR;

namespace ECommerce.Application.Features.Catalog.Categories.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;
