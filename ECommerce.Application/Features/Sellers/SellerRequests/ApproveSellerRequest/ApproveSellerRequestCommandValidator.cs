using FluentValidation;

namespace ECommerce.Application.Features.Sellers.SellerRequests.ApproveSellerRequest;

public sealed class ApproveSellerRequestCommandValidator : AbstractValidator<ApproveSellerRequestCommand>
{
    public ApproveSellerRequestCommandValidator()
    {
        RuleFor(x => x.SellerRequestId).NotEmpty();
    }
}