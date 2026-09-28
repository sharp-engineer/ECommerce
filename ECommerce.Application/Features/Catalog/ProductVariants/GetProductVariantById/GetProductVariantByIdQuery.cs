using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.GetProductVariantById;

public sealed record GetProductVariantByIdQuery(Guid Id) : IRequest<ProductVariantDto>;