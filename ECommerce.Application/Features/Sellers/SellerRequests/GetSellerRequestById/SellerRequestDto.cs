using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Sellers.SellerRequests.GetSellerRequestById;

public sealed record SellerRequestDto(
    Guid Id,
    Guid UserId,
    SellerRequestStatus Status,
    string? Reason,
    Guid? ReviewedByUserId,
    DateTime? ReviewedAt);