using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferStock;

public sealed record UpdateSellerOfferStockCommand(Guid SellerOfferId, int Stock, uint RowVersion) : IRequest;