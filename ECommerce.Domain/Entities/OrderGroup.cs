using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class OrderGroup : AuditableEntity
{
    //ctor's
    private OrderGroup()
    {

    }

    internal OrderGroup(Guid sellerId)
    {
        if (sellerId == Guid.Empty)
            throw new DomainException("Seller ID is required.");

        SellerId = sellerId;
        Status = OrderGroupStatus.Pending;
    }

    //prop's
    public Guid OrderId { get; private set; }
    public Guid SellerId { get; private set; }
    public OrderGroupStatus Status { get; set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();


    //method's
    internal void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(x => x.TotalAmount);
    }
}