using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class SellerOfferRepositoryTests
{
     [Test]
    public async Task GetByIdForUpdateAsync_should_use_expected_row_version_for_concurrency()
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
                "No ProductVariant exists for the test.");

            var seller = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = $"repository-test-{Guid.NewGuid():N}",
                Email = $"repository-test-{Guid.NewGuid():N}@test.local",
                EmailConfirmed = true,
                FirstName = "Repository",
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
            uint staleRowVersion;

            // Load the original version.
            await using (var contextA = new ECommerceDbContext(options))
            {
                var offer = await contextA.SellerOffers
                    .SingleAsync(x => x.Id == sellerOfferId);

                staleRowVersion = offer.RowVersion;
            }

            // Another request modifies the record first.
            await using (var contextB = new ECommerceDbContext(options))
            {
                var offer = await contextB.SellerOffers
                    .SingleAsync(x => x.Id == sellerOfferId);

                offer.UpdatePrice(offer.Price + 1);

                await contextB.SaveChangesAsync();
            }

            // Resolve the real repository through DI.
            var configuration = new ConfigurationManager();
            configuration["ConnectionStrings:ECommerceDb"] = connectionString;
            configuration["Jwt:Issuer"] = "ECommerce.Tests";
            configuration["Jwt:Audience"] = "ECommerce.Tests";
            configuration["Jwt:ExpirationMinutes"] = "60";
            configuration["Jwt:SecretKey"] = new string('t', 32);

            var services = new ServiceCollection();

            services.AddInfrastructure(configuration);

            await using var serviceProvider =
                services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var repository =
                scope.ServiceProvider
                    .GetRequiredService<ISellerOfferRepository>();

            var unitOfWork =
                scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

            // Repository must load the current row,
            // but use the stale version as OriginalValue.
            var offerForUpdate =
                await repository.GetByIdForUpdateAsync(
                    sellerOfferId,
                    staleRowVersion);

            Assert.That(offerForUpdate, Is.Not.Null);

            Assert.That(
                offerForUpdate!.RowVersion,
                Is.Not.EqualTo(staleRowVersion));

            offerForUpdate.UpdateStock(
                offerForUpdate.Stock + 1);

            var exception =
                Assert.ThrowsAsync<ConcurrencyConflictException>(
                    async () =>
                        await unitOfWork.SaveChangesAsync());

            Assert.That(
                exception!.InnerException,
                Is.TypeOf<DbUpdateConcurrencyException>());
        }
        finally
        {
            await using var cleanupContext =
                new ECommerceDbContext(options);

            var sellerOffer =
                await cleanupContext.SellerOffers
                    .SingleOrDefaultAsync(
                        x => x.Id == sellerOfferId);

            if (sellerOffer is not null)
                cleanupContext.SellerOffers.Remove(sellerOffer);

            var seller =
                await cleanupContext.Users
                    .SingleOrDefaultAsync(
                        x => x.Id == sellerId);

            if (seller is not null)
                cleanupContext.Users.Remove(seller);

            await cleanupContext.SaveChangesAsync();
        }
    }
}