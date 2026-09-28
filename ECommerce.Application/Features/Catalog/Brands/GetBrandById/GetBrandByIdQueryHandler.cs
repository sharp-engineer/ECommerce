using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Catalog.Brands.GetBrandById;

public sealed class GetBrandByIdQueryHandler(IBrandRepository brandRepository)
    : IRequestHandler<GetBrandByIdQuery, BrandDto>
{
    public async Task<BrandDto> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(query.Id, cancellationToken);
        return brand is null
            ? throw new NotFoundException($"Brand with id '{query.Id}' was not found.")
            : new BrandDto(brand.Id, brand.Name);
    }
}