using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Enums;
using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.RejectSellerRequest;

public sealed class RejectSellerRequestCommandHandler(
    ISellerRequestRepository sellerRequestRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IRequestHandler<RejectSellerRequestCommand>
{
    public async Task Handle(RejectSellerRequestCommand command, CancellationToken cancellationToken)
    {
        var request = await sellerRequestRepository.GetByIdAsync(command.SellerRequestId, cancellationToken);
        if (request is null)
            throw new NotFoundException($"Seller request with id '{command.SellerRequestId}' was not found.");

        var adminUserId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");
        
        request.Reject(adminUserId);
        await userRepository.SetSellerStatusAsync(request.UserId, SellerStatus.Rejected, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}