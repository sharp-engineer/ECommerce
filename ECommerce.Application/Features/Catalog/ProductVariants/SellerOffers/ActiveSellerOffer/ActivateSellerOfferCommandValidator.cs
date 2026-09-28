using FluentValidation;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.ActiveSellerOffer;

public sealed class ActivateSellerOfferCommandValidator : AbstractValidator<ActivateSellerOfferCommand>
{
    public ActivateSellerOfferCommandValidator()
    {
        RuleFor(x => x.SellerOfferId).NotEmpty();
        RuleFor(x => x.RowVersion).NotEqual(0u);
    }
}