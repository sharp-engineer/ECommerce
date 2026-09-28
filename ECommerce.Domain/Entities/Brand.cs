using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Brand : AuditableEntity
{
    //ctor's
    private Brand()
    {
        
    }

    private Brand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Brand name is required.");
        
        Name = name;
    }
    
    
    //prop's
    public string Name { get; private set; } = null!;
    
    
    //method's
    public static Brand Create(string name)
    {
        return new Brand(name);
    }
}