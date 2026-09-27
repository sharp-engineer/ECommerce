using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.GetSellerRequestById;

public sealed class GetSellerRequestByIdQueryHandler(ISellerRequestRepository sellerRequestRepository,ICurrentUser currentUser)
    : IRequestHandler<GetSellerRequestByIdQuery, SellerRequestDto?>
{
    public async Task<SellerRequestDto?> Handle(GetSellerRequestByIdQuery query, CancellationToken cancellationToken)
    {
        var sellerRequest = await sellerRequestRepository.GetByIdAsync(query.Id, cancellationToken);
        if (sellerRequest is null)
            return null;
        
        var currentUserId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");
        var isAdmin = currentUser.IsInRole(ApplicationRoles.Admin);
        if(!isAdmin && sellerRequest.UserId != currentUserId)
            throw new ForbiddenException("You are not allowed to view this seller request.");

        return new SellerRequestDto(
            sellerRequest.Id,
            sellerRequest.UserId,
            sellerRequest.Status,
            sellerRequest.Reason,
            sellerRequest.ReviewedByUserId,
            sellerRequest.ReviewedAt);
    }
}