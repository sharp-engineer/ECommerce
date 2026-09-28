using FluentValidation;

namespace ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferStock;

public sealed class UpdateSellerOfferStockCommandValidator : AbstractValidator<UpdateSellerOfferStockCommand>
{
    public UpdateSellerOfferStockCommandValidator()
    {
        RuleFor(x => x.SellerOfferId).NotEmpty();
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RowVersion).NotEqual(0u);
    }
}