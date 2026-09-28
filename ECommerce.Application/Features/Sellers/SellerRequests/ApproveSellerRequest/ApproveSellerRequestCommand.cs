using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.ApproveSellerRequest;

public sealed record ApproveSellerRequestCommand(Guid SellerRequestId) : IRequest;