using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.ApproveSellerRequest;

public sealed class ApproveSellerRequestCommandHandler(
    ISellerRequestRepository sellerRequestRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<ApproveSellerRequestCommand>
{
    public async Task Handle(ApproveSellerRequestCommand command, CancellationToken cancellationToken)
    {
        var request = await sellerRequestRepository.GetByIdAsync(command.SellerRequestId, cancellationToken);
        if (request is null)
            throw new NotFoundException($"Seller request with id '{command.SellerRequestId}' was not found.");

        var adminUserId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");

        request.Approve(adminUserId);
        await userRepository.SetSellerStatusAsync(request.UserId, SellerStatus.Active, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}