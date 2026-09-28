using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.ActiveSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.CreateSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.DeactivateSellerOffer;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferPrice;
using ECommerce.Application.Features.Catalog.ProductVariants.SellerOffers.UpdateSellerOfferStock;

namespace ECommerce.Tests.Application.Features.Catalog.ProductVariants.SellerOffers;

public sealed class SellerOfferValidatorTests
{
    [Test]
    public void CreateSellerOffer_should_accept_valid_command()
    {
        var validator = new CreateSellerOfferCommandValidator();

        var command = new CreateSellerOfferCommand(
            Guid.NewGuid(),
            100_000,
            0);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void CreateSellerOffer_should_reject_empty_seller_id()
    {
        var validator = new CreateSellerOfferCommandValidator();

        var command = new CreateSellerOfferCommand(
            Guid.Empty,
            100_000,
            10);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void CreateSellerOffer_should_reject_empty_product_variant_id()
    {
        var validator = new CreateSellerOfferCommandValidator();

        var command = new CreateSellerOfferCommand(
            Guid.Empty,
            100_000,
            10);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void CreateSellerOffer_should_reject_non_positive_price()
    {
        var validator = new CreateSellerOfferCommandValidator();

        var command = new CreateSellerOfferCommand(
            Guid.NewGuid(),
            0,
            10);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void CreateSellerOffer_should_reject_negative_stock()
    {
        var validator = new CreateSellerOfferCommandValidator();

        var command = new CreateSellerOfferCommand(
            Guid.NewGuid(),
            100_000,
            -1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void UpdateSellerOfferPrice_should_accept_valid_command()
    {
        var validator = new UpdateSellerOfferPriceCommandValidator();

        var command = new UpdateSellerOfferPriceCommand(
            Guid.NewGuid(),
            150_000,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void UpdateSellerOfferPrice_should_reject_empty_offer_id()
    {
        var validator = new UpdateSellerOfferPriceCommandValidator();

        var command = new UpdateSellerOfferPriceCommand(
            Guid.Empty,
            150_000,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void UpdateSellerOfferPrice_should_reject_non_positive_price()
    {
        var validator = new UpdateSellerOfferPriceCommandValidator();

        var command = new UpdateSellerOfferPriceCommand(
            Guid.NewGuid(),
            0,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void UpdateSellerOfferPrice_should_reject_zero_row_version()
    {
        var validator = new UpdateSellerOfferPriceCommandValidator();

        var command = new UpdateSellerOfferPriceCommand(
            Guid.NewGuid(),
            150_000,
            0);

        var result = validator.Validate(command);
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void UpdateSellerOfferStock_should_accept_zero_stock()
    {
        var validator = new UpdateSellerOfferStockCommandValidator();

        var command = new UpdateSellerOfferStockCommand(
            Guid.NewGuid(),
            0,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void UpdateSellerOfferStock_should_reject_negative_stock()
    {
        var validator = new UpdateSellerOfferStockCommandValidator();

        var command = new UpdateSellerOfferStockCommand(
            Guid.NewGuid(),
            -1,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void UpdateSellerOfferStock_should_reject_zero_row_version()
    {
        var validator = new UpdateSellerOfferStockCommandValidator();

        var command = new UpdateSellerOfferStockCommand(
            Guid.NewGuid(),
            10,
            0);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void ActivateSellerOffer_should_accept_valid_command()
    {
        var validator = new ActivateSellerOfferCommandValidator();

        var command = new ActivateSellerOfferCommand(
            Guid.NewGuid(),
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void ActivateSellerOffer_should_reject_empty_offer_id()
    {
        var validator = new ActivateSellerOfferCommandValidator();

        var command = new ActivateSellerOfferCommand(
            Guid.Empty,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void ActivateSellerOffer_should_reject_zero_row_version()
    {
        var validator = new ActivateSellerOfferCommandValidator();

        var command = new ActivateSellerOfferCommand(
            Guid.NewGuid(),
            0);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void DeactivateSellerOffer_should_accept_valid_command()
    {
        var validator = new DeactivateSellerOfferCommandValidator();

        var command = new DeactivateSellerOfferCommand(
            Guid.NewGuid(),
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void DeactivateSellerOffer_should_reject_empty_offer_id()
    {
        var validator = new DeactivateSellerOfferCommandValidator();

        var command = new DeactivateSellerOfferCommand(
            Guid.Empty,
            1);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void DeactivateSellerOffer_should_reject_zero_row_version()
    {
        var validator = new DeactivateSellerOfferCommandValidator();

        var command = new DeactivateSellerOfferCommand(
            Guid.NewGuid(),
            0);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }
}