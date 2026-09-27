using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.CreateSellerRequest;

public sealed class CreateSellerRequestCommandHandler(
    ISellerRequestRepository sellerRequestRepository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSellerRequestCommand, Guid>
{
    public async Task<Guid> Handle(CreateSellerRequestCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedException("Authenticated user is required.");

        var sellerRequest = SellerRequest.Create(userId, command.Reason);
        await sellerRequestRepository.AddAsync(sellerRequest, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return sellerRequest.Id;
    }
}