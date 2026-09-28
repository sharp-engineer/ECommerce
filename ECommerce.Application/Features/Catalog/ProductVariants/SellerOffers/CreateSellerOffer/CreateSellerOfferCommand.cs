using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.CreateSellerOffer;

public sealed record CreateSellerOfferCommand(Guid ProductVariantId, decimal Price, int Stock)
    : IRequest<Guid>;