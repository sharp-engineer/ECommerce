using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductImages.GetProductImageById;

public sealed record GetProductImageByIdQuery(Guid Id) : IRequest<ProductImageDto>;