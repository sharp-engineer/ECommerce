using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferPrice;

public sealed record UpdateSellerOfferPriceCommand(Guid SellerOfferId, decimal Price, uint RowVersion) : IRequest;