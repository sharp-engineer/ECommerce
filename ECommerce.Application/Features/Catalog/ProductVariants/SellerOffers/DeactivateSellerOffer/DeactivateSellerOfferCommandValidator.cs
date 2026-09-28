using FluentValidation;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.DeactivateSellerOffer;

public sealed class DeactivateSellerOfferCommandValidator : AbstractValidator<DeactivateSellerOfferCommand>
{
    public DeactivateSellerOfferCommandValidator()
    {
        RuleFor(x => x.SellerOfferId).NotEmpty();
        RuleFor(x => x.RowVersion).NotEqual(0u);
    }
}