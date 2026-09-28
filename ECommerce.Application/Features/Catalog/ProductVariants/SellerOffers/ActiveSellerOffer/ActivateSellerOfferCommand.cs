using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.ActiveSellerOffer;

public sealed record ActivateSellerOfferCommand(Guid SellerOfferId, uint RowVersion) : IRequest;