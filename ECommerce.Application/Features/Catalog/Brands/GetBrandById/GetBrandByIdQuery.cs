using ECommerce.Application.Features.Catalog.Products.GetProductById;
using MediatR;

namespace ECommerce.Application.Features.Catalog.Brands.GetBrandById;

public sealed record GetBrandByIdQuery(Guid Id) : IRequest<BrandDto>;