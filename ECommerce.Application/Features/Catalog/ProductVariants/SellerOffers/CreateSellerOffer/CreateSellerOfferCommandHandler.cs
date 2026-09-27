using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.CreateSellerOffer;

public class CreateSellerOfferCommandHandler(
    ICurrentUser currentUser,
    IProductVariantRepository productVariantRepository,
    ISellerOfferRepository sellerOfferRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSellerOfferCommand, Guid>
{
    public async Task<Guid> Handle(CreateSellerOfferCommand command, CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");

        var variantExist = await productVariantRepository.ExistsAsync(command.ProductVariantId, cancellationToken);
        if (!variantExist)
            throw new NotFoundException($"Product variant with id '{command.ProductVariantId}' was not found.");

        var sellerOffer = SellerOffer.Create(
            sellerId,
            command.ProductVariantId,
            command.Price,
            command.Stock);

        await sellerOfferRepository.AddAsync(sellerOffer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return sellerOffer.Id;
    }
}