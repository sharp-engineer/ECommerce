using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class OrderItem : Entity
{
    //ctor's
    private OrderItem()
    {

    }

    internal OrderItem(Guid productId, Guid productVariantId, Guid sellerOfferId, string productName,
        string variantName, string sellerName, decimal unitPrice, int quantity, decimal discountAmount)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product ID is required.");

        if (productVariantId == Guid.Empty)
            throw new DomainException("Product variant ID is required.");

        if (sellerOfferId == Guid.Empty)
            throw new DomainException("Seller offer ID is required.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        if (string.IsNullOrWhiteSpace(variantName))
            throw new DomainException("Variant name is required.");

        if (string.IsNullOrWhiteSpace(sellerName))
            throw new DomainException("Seller name is required.");

        if (unitPrice < 0)
            throw new DomainException("Unit price cannot be negative.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (discountAmount < 0)
            throw new DomainException("Discount amount cannot be negative.");

        ProductId = productId;
        ProductVariantId = productVariantId;
        SellerOfferId = sellerOfferId;

        ProductName = productName;
        VariantName = variantName;
        SellerName = sellerName;

        UnitPrice = unitPrice;
        Quantity = quantity;
        DiscountAmount = discountAmount;

        var subtotal = unitPrice * quantity;
        if(discountAmount > subtotal)
            throw new DomainException("Discount amount cannot exceed subtotal.");

        TotalAmount = subtotal - discountAmount;
    }


    //prop's
    public Guid OrderGroupId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public Guid SellerOfferId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string VariantName { get; private set; } = null!;
    public string SellerName { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
}