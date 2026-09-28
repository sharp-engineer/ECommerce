using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories;

internal sealed class SellerRequestRepository(ECommerceDbContext dbContext) : ISellerRequestRepository
{
    public async Task<SellerRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.SellerRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(SellerRequest sellerRequest, CancellationToken cancellationToken = default)
    {
        await dbContext.SellerRequests.AddAsync(sellerRequest, cancellationToken);
    }
}