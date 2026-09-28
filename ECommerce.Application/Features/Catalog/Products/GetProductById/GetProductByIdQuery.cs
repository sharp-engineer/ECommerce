using MediatR;

namespace ECommerce.Application.Features.Catalog.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDetailsDto>;