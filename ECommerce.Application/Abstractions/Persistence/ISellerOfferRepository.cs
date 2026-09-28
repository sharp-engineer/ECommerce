using ECommerce.Domain.Entities;

namespace ECommerce.Application.Abstractions.Persistence;

public interface ISellerOfferRepository
{
    Task AddAsync(SellerOffer sellerOffer, CancellationToken cancellationToken = default);
    Task<SellerOffer?> GetByIdAsync(Guid sellerOfferId, CancellationToken cancellationToken = default);
    Task<SellerOffer?> GetByIdForUpdateAsync(Guid sellerOfferId, uint expectedRowVersion,
        CancellationToken cancellationToken = default);
}