using FluentValidation;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferPrice;

public sealed class UpdateSellerOfferPriceCommandValidator : AbstractValidator<UpdateSellerOfferPriceCommand>
{
    public UpdateSellerOfferPriceCommandValidator()
    {
        RuleFor(x => x.SellerOfferId).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.RowVersion).NotEqual(0u);
    }
}