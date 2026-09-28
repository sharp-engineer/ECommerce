using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class SellerRequest : Entity
{
    //ctor's
    private SellerRequest()
    {

    }

    public SellerRequest(Guid userId, string? reason)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID is required.");

        UserId = userId;
        Reason = reason;
        Status = SellerRequestStatus.Pending;
    }


    //prop's
    public Guid UserId { get; private set; }
    public SellerRequestStatus Status { get; private set; }
    public string? Reason { get; private set; }
    public Guid? ReviewedByUserId { get; private set; }
    public DateTime? ReviewedAt { get; private set; }


    //method's
    public static SellerRequest Create(Guid userId, string? reason)
    {
        return new SellerRequest(userId, reason);
    }

    public void Approve(Guid adminUserId)
    {
        if (adminUserId == Guid.Empty)
            throw new DomainException("Admin user ID is required.");

        if (Status != SellerRequestStatus.Pending)
            throw new DomainException("Only pending request can be approved.");

        Status = SellerRequestStatus.Approved;
        ReviewedByUserId = adminUserId;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Reject(Guid adminUserId)
    {
        if (adminUserId == Guid.Empty)
            throw new DomainException("Admin user ID is required.");

        if (Status != SellerRequestStatus.Pending)
            throw new DomainException("Only pending request can be rejected.");

        Status = SellerRequestStatus.Rejected;
        ReviewedByUserId = adminUserId;
        ReviewedAt = DateTime.UtcNow;
    }
}