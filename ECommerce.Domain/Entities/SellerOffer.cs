using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class SellerOffer : AuditableEntity
{
    //ctor's
    private SellerOffer()
    {

    }

    private SellerOffer(Guid sellerId, Guid productVariantId, decimal price, int stock)
    {
        if (sellerId == Guid.Empty)
            throw new DomainException("Seller ID is required.");

        if (productVariantId == Guid.Empty)
            throw new DomainException("Product variant ID is required.");

        if (price <= 0)
            throw new DomainException("Price must be greater than zero.");

        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        SellerId = sellerId;
        ProductVariantId = productVariantId;
        Price = price;
        Stock = stock;
        Status = SellerOfferStatus.Inactive;
    }


    //prop's
    public Guid SellerId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = null!;

    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public SellerOfferStatus Status { get; private set; }
    public uint RowVersion { get; private set; }


    //method's
    public static SellerOffer Create(Guid sellerId, Guid productVariantId, decimal price, int stock)
    {
        return new SellerOffer(
            sellerId,
            productVariantId,
            price,
            stock);
    }

    public void UpdatePrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be greater than zero.");

        Price = price;
    }

    public void UpdateStock(int stock)
    {
        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        Stock = stock;
    }

    public void Activate()
    {
        if(Status == SellerOfferStatus.Active)
            throw new DomainException("Seller offer is already active.");
        
        Status = SellerOfferStatus.Active;
    }

    public void Deactivate()
    {
        if(Status == SellerOfferStatus.Inactive)
            throw new DomainException("Seller offer is already inactive.");
        
        Status = SellerOfferStatus.Inactive;
    }
}