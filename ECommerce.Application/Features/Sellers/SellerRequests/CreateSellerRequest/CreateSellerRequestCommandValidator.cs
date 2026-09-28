using FluentValidation;

namespace ECommerce.Application.Features.Sellers.SellerRequests.CreateSellerRequest;

public sealed class CreateSellerRequestCommandValidator : AbstractValidator<CreateSellerRequestCommand>
{
    public CreateSellerRequestCommandValidator()
    {
        RuleFor(x => x.Reason).MaximumLength(1000)
            .When(x => x.Reason is not null);
    }
}