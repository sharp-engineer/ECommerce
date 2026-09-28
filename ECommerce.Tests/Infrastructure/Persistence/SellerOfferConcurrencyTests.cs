using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class SellerOfferConcurrencyTests : PostgresTestBase
{
    [Test]
    public async Task Updating_same_seller_offer_from_two_contexts_should_throw_concurrency_exception()
    {
        Guid sellerOfferId;

        // Arrange
        await using (var setupContext = CreateDbContext())
        {
            var brand = Brand.Create(
                $"Concurrency Test Brand {Guid.NewGuid():N}");

            var category = Category.Create(
                $"Concurrency Test Category {Guid.NewGuid():N}");

            var product = Product.Create(
                $"Concurrency Test Product {Guid.NewGuid():N}",
                "Concurrency test product.",
                brand.Id,
                category.Id);

            var productVariant = ProductVariant.Create(
                product.Id,
                $"SKU-{Guid.NewGuid():N}",
                "Concurrency Test Variant");

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

        await using var contextA = CreateDbContext();
        await using var contextB = CreateDbContext();

        var offerA = await contextA.SellerOffers
            .SingleAsync(x => x.Id == sellerOfferId);

        var offerB = await contextB.SellerOffers
            .SingleAsync(x => x.Id == sellerOfferId);

        Assert.That(
            offerA.RowVersion,
            Is.EqualTo(offerB.RowVersion));

        var initialRowVersion = offerA.RowVersion;

        // First update succeeds.
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
            Is.TypeOf<DbUpdateConcurrencyException>());
    }
}