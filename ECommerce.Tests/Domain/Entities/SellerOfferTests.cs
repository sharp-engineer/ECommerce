using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Tests.Domain.Entities;

public sealed class SellerOfferTests
{
    [Test]
    public void Create_should_create_inactive_offer()
    {
        var sellerId = Guid.NewGuid();
        var productVariantId = Guid.NewGuid();

        var offer = SellerOffer.Create(
            sellerId,
            productVariantId,
            100_000,
            10);

        Assert.Multiple(() =>
        {
            Assert.That(offer.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(offer.SellerId, Is.EqualTo(sellerId));
            Assert.That(offer.ProductVariantId, Is.EqualTo(productVariantId));
            Assert.That(offer.Price, Is.EqualTo(100_000));
            Assert.That(offer.Stock, Is.EqualTo(10));
            Assert.That(offer.Status, Is.EqualTo(SellerOfferStatus.Inactive));
        });
    }

    [Test]
    public void Create_should_reject_empty_seller_id()
    {
        Assert.Throws<DomainException>(() =>
            SellerOffer.Create(
                Guid.Empty,
                Guid.NewGuid(),
                100_000,
                10));
    }

    [Test]
    public void Create_should_reject_empty_product_variant_id()
    {
        Assert.Throws<DomainException>(() =>
            SellerOffer.Create(
                Guid.NewGuid(),
                Guid.Empty,
                100_000,
                10));
    }

    [Test]
    public void Create_should_reject_non_positive_price()
    {
        Assert.Throws<DomainException>(() =>
            SellerOffer.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                0,
                10));
    }

    [Test]
    public void Create_should_reject_negative_stock()
    {
        Assert.Throws<DomainException>(() =>
            SellerOffer.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                100_000,
                -1));
    }

    [Test]
    public void UpdatePrice_should_change_price()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        offer.UpdatePrice(150_000);

        Assert.That(offer.Price, Is.EqualTo(150_000));
    }

    [Test]
    public void UpdatePrice_should_reject_non_positive_price()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        Assert.Throws<DomainException>(() =>
            offer.UpdatePrice(0));
    }

    [Test]
    public void UpdateStock_should_change_stock()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        offer.UpdateStock(0);

        Assert.That(offer.Stock, Is.EqualTo(0));
    }

    [Test]
    public void UpdateStock_should_reject_negative_stock()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        Assert.Throws<DomainException>(() =>
            offer.UpdateStock(-1));
    }

    [Test]
    public void Activate_should_change_status_to_active()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        offer.Activate();

        Assert.That(offer.Status, Is.EqualTo(SellerOfferStatus.Active));
    }

    [Test]
    public void Activate_should_reject_already_active_offer()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        offer.Activate();

        Assert.Throws<DomainException>(() =>
            offer.Activate());
    }

    [Test]
    public void Deactivate_should_change_status_to_inactive()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        offer.Activate();
        offer.Deactivate();

        Assert.That(offer.Status,Is.EqualTo(SellerOfferStatus.Inactive));
    }

    [Test]
    public void Deactivate_should_reject_already_inactive_offer()
    {
        var offer = SellerOffer.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100_000,
            10);

        Assert.Throws<DomainException>(() =>
            offer.Deactivate());
    }
}