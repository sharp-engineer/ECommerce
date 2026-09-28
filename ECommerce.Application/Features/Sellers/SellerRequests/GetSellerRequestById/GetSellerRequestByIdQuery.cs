using MediatR;

namespace ECommerce.Application.Features.Sellers.SellerRequests.GetSellerRequestById;

public sealed record GetSellerRequestByIdQuery(Guid Id) : IRequest<SellerRequestDto>;