using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferPrice;

public sealed class UpdateSellerOfferPriceCommandHandler(
    ISellerOfferRepository sellerOfferRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<UpdateSellerOfferPriceCommand>
{
    public async Task Handle(UpdateSellerOfferPriceCommand command, CancellationToken cancellationToken)
    {
        var sellerOffer =
            await sellerOfferRepository.GetByIdForUpdateAsync(command.SellerOfferId, command.RowVersion,
                cancellationToken);
        if (sellerOffer is null)
            throw new NotFoundException($"Seller offer with id '{command.SellerOfferId}' was not found.");

        var currentUserId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");
        if (sellerOffer.SellerId != currentUserId)
            throw new ForbiddenException("You are not allowed to modify this seller offer.");
        
        sellerOffer.UpdatePrice(command.Price);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}