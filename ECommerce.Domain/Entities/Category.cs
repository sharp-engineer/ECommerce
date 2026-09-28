using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Category : AuditableEntity
{
    //ctor's
    private Category()
    {

    }

    private Category(string name, Guid? parentCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");

        Name = name.Trim();
        ParentCategoryId = parentCategoryId == Guid.Empty ? null : parentCategoryId;
    }


    //prop's
    public string Name { get; private set; } = null!;

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    private readonly List<Category> _children = [];
    public IReadOnlyCollection<Category> Children => _children.AsReadOnly();


    //method's
    public static Category Create(string name, Guid? parentCategoryId = null) => new(name, parentCategoryId);
}