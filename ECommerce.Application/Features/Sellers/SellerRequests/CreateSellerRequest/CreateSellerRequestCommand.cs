using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.CreateSellerRequest;

public sealed record CreateSellerRequestCommand(string? Reason) : IRequest<Guid>;