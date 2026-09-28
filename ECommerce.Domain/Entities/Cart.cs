using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Cart : AuditableEntity
{
    //ctor's
    private Cart()
    {

    }

    public Cart(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID is required.");

        UserId = userId;
    }


    //prop's
    public Guid UserId { get; private set; }

    private readonly List<CartItem> _items = [];
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();


    //method's
    public static Cart Create(Guid userId)
    {
        return new Cart(userId);
    }

    public void AddItem(Guid sellerOfferId, int quantity)
    {
        if (sellerOfferId == Guid.Empty)
            throw new DomainException("Seller offer ID is required.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var existingItem = _items.FirstOrDefault(x => x.SellerOfferId == sellerOfferId);
        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        _items.Add(new CartItem(sellerOfferId, quantity));
    }

    public void UpdateItem(Guid sellerOfferId, int quantity)
    {
        var item = _items.FirstOrDefault(x => x.SellerOfferId == sellerOfferId);
        if (item is null)
            throw new DomainException("Cart item was not found.");

        item.SetQuantity(quantity);
    }

    public void RemoveItem(Guid sellerOfferId)
    {
        var item = _items.FirstOrDefault(x => x.SellerOfferId == sellerOfferId);
        if (item is null)
            return;

        _items.Remove(item);
    }
}