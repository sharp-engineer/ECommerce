using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.DeactivateSellerOffer;

public sealed record DeactivateSellerOfferCommand(Guid SellerOfferId, uint RowVersion) : IRequest;