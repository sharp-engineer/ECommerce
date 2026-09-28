using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class SellerOfferConcurrencyTests
{
    [Test]
    public async Task Updating_same_seller_offer_from_two_contexts_should_throw_concurrency_exception()
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ECOMMERCE_TEST_CONNECTION_STRING");

        Assert.That(connectionString, Is.Not.Null.And.Not.Empty);

        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        Guid sellerId;
        Guid sellerOfferId;

        // Arrange
        await using (var setupContext = new ECommerceDbContext(options))
        {
            var productVariant = await setupContext.ProductVariants
                .FirstOrDefaultAsync();

            Assert.That(
                productVariant,
                Is.Not.Null,
                "No ProductVariant exists for the concurrency test.");

            var seller = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = $"concurrency-test-{Guid.NewGuid():N}",
                Email = $"concurrency-test-{Guid.NewGuid():N}@test.local",
                EmailConfirmed = true,
                FirstName = "Concurrency",
                LastName = "Test"
            };

            setupContext.Users.Add(seller);

            var sellerOffer = SellerOffer.Create(
                seller.Id,
                productVariant!.Id,
                100_000,
                10);

            setupContext.SellerOffers.Add(sellerOffer);

            await setupContext.SaveChangesAsync();

            sellerId = seller.Id;
            sellerOfferId = sellerOffer.Id;
        }

        try
        {
            await using var contextA = new ECommerceDbContext(options);
            await using var contextB = new ECommerceDbContext(options);

            var offerA = await contextA.SellerOffers
                .SingleAsync(x => x.Id == sellerOfferId);

            var offerB = await contextB.SellerOffers
                .SingleAsync(x => x.Id == sellerOfferId);

            Assert.That(
                offerA.RowVersion,
                Is.EqualTo(offerB.RowVersion));

            var initialRowVersion = offerA.RowVersion;

            // First concurrent update succeeds.
            offerA.UpdatePrice(offerA.Price + 1);

            await contextA.SaveChangesAsync();

            Assert.That(
                offerA.RowVersion,
                Is.Not.EqualTo(initialRowVersion));

            // Second context still has the stale RowVersion.
            offerB.UpdatePrice(offerB.Price + 2);

            var exception = Assert.ThrowsAsync<ConcurrencyConflictException>(
                async () => await contextB.SaveChangesAsync());

            Assert.That(
                exception!.InnerException,
                Is.TypeOf<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>());
        }
        finally
        {
            // Cleanup SellerOffer first, then the temporary seller.
            await using var cleanupContext = new ECommerceDbContext(options);

            var sellerOffer = await cleanupContext.SellerOffers
                .SingleOrDefaultAsync(x => x.Id == sellerOfferId);

            if (sellerOffer is not null)
                cleanupContext.SellerOffers.Remove(sellerOffer);

            var seller = await cleanupContext.Users
                .SingleOrDefaultAsync(x => x.Id == sellerId);

            if (seller is not null)
                cleanupContext.Users.Remove(seller);

            await cleanupContext.SaveChangesAsync();
        }
    }
}