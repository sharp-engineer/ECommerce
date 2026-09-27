using FluentValidation;

namespace ECommerce.Application.Features.Sellers.SellerRequests.RejectSellerRequest;

public sealed class RejectSellerRequestCommandValidator : AbstractValidator<RejectSellerRequestCommand>
{
    public RejectSellerRequestCommandValidator()
    {
        RuleFor(x => x.SellerRequestId).NotEmpty();
    }
}