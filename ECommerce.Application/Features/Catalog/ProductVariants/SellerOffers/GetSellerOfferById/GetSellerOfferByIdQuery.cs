using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.GetSellerOfferById;

public sealed record GetSellerOfferByIdQuery(Guid Id) : IRequest<SellerOfferDto>;