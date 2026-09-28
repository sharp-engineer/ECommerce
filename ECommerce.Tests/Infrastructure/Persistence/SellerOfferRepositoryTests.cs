using ECommerce.Application.Abstractions.Persistence;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class SellerOfferRepositoryTests : PostgresTestBase
{
    [Test]
    public async Task GetByIdForUpdateAsync_should_use_expected_row_version_for_concurrency()
    {
        Guid sellerOfferId;

        // Arrange
        await using (var setupContext = CreateDbContext())
        {
            var brand = Brand.Create(
                $"Test Brand {Guid.NewGuid():N}");

            var category = Category.Create(
                $"Test Category {Guid.NewGuid():N}");

            var product = Product.Create(
                $"Test Product {Guid.NewGuid():N}",
                "Repository concurrency test product.",
                brand.Id,
                category.Id);

            var productVariant = ProductVariant.Create(
                product.Id,
                $"SKU-{Guid.NewGuid():N}",
                "Test Variant");

            setupContext.Brands.Add(brand);
            setupContext.Categories.Add(category);
            setupContext.Products.Add(product);
            setupContext.ProductVariants.Add(productVariant);

            var sellerOffer = SellerOffer.Create(
                Guid.NewGuid(),
                productVariant.Id,
                100_000,
                10);

            setupContext.SellerOffers.Add(sellerOffer);

            await setupContext.SaveChangesAsync();

            sellerOfferId = sellerOffer.Id;
        }

        uint staleRowVersion;

        // Load the original row version.
        await using (var contextA = CreateDbContext())
        {
            var offer = await contextA.SellerOffers
                .SingleAsync(x => x.Id == sellerOfferId);

            staleRowVersion = offer.RowVersion;
        }

        // Simulate another request updating the same offer first.
        await using (var contextB = CreateDbContext())
        {
            var offer = await contextB.SellerOffers
                .SingleAsync(x => x.Id == sellerOfferId);

            offer.UpdatePrice(offer.Price + 1);

            await contextB.SaveChangesAsync();
        }

        // Resolve the real repository through the application's DI setup.
        var configuration = new ConfigurationManager();

        configuration["ConnectionStrings:ECommerceDb"] = ConnectionString;
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

        // The repository must load the current row,
        // while the supplied stale version becomes EF's OriginalValue.
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
}