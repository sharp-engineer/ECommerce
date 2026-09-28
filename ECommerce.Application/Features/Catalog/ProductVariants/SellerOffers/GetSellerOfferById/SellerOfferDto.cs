using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.GetSellerOfferById;

public sealed record SellerOfferDto(
    Guid Id,
    Guid SellerId,
    decimal Price,
    int Stock,
    SellerOfferStatus Status,
    uint RowVersion);