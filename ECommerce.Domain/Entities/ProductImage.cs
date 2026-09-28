using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class ProductImage : AuditableEntity
{
    //ctor's
    private ProductImage()
    {

    }

    private ProductImage(Guid productId, string url, string? altText, int sortOrder, bool isPrimary,
        Guid? productVariantId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id is required.");

        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Image URL is required.");

        if (sortOrder < 0)
            throw new DomainException("Sort Order cannot be negative.");

        ProductId = productId;
        ProductVariantId = productVariantId;
        Url = url.Trim();
        AltText = altText;
        SortOrder = sortOrder;
        IsPrimary = isPrimary;
    }


    //prop's
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Guid? ProductVariantId { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }

    public string Url { get; private set; } = null!;
    public string? AltText { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsPrimary { get; private set; }


    //method's
    public static ProductImage Create(Guid productId, string url, string? altText, int sortOrder, bool isPrimary,
        Guid? productVariantId = null) => new(productId, url, altText, sortOrder, isPrimary, productVariantId);
}