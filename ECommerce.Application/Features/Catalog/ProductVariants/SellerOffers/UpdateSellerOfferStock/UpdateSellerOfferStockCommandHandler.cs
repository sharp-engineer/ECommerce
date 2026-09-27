using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferStock;

public sealed class UpdateSellerOfferStockCommandHandler(
    ISellerOfferRepository sellerOfferRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<UpdateSellerOfferStockCommand>
{
    public async Task Handle(UpdateSellerOfferStockCommand command, CancellationToken cancellationToken)
    {
        var sellerOffer =
            await sellerOfferRepository.GetByIdForUpdateAsync(command.SellerOfferId, command.RowVersion,
                cancellationToken);
        if (sellerOffer is null)
            throw new NotFoundException($"Seller offer with id '{command.SellerOfferId}' was not found.");

        var currentUserId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");
        if (sellerOffer.SellerId != currentUserId)
            throw new ForbiddenException("You are not allowed to modify this seller offer.");

        sellerOffer.UpdateStock(command.Stock);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}