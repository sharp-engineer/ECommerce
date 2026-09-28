using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Payment : AuditableEntity
{
    //ctor's
    private Payment()
    {

    }

    private Payment(Guid orderId, decimal amount, PaymentMethod paymentMethod)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order ID is required.");

        if (amount <= 0)
            throw new DomainException("Payment amount must be greater than zero.");

        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = PaymentStatus.Pending;
    }


    //prop's
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public string? TransactionId { get; private set; }
    public string? GatewayReference { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? FailedAt { get; private set; }


    //method's
    public static Payment Create(Guid orderId, decimal amount, PaymentMethod paymentMethod)
    {
        return new Payment(orderId, amount, paymentMethod);
    }

    public void MarkSuccessful(string transactionId, string? gatewayReference = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payment can be marked as successful.");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new DomainException("Transaction ID is required.");

        Status = PaymentStatus.Successful;
        TransactionId = transactionId;
        GatewayReference = gatewayReference;
        PaidAt = DateTime.UtcNow;
    }

    public void MockFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payment can be marked as failed.");

        Status = PaymentStatus.Failed;
        FailedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Only pending payments can be cancelled.");

        Status = PaymentStatus.Cancelled;
    }
}