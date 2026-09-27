using ECommerce.Application.Features.Sellers.SellerRequests.ApproveSellerRequest;
using ECommerce.Application.Features.Sellers.SellerRequests.CreateSellerRequest;
using ECommerce.Application.Features.Sellers.SellerRequests.RejectSellerRequest;

namespace ECommerce.Tests.Application.Features.Sellers.SellerRequests;

public sealed class SellerRequestValidatorTests
{
    [Test]
    public void CreateSellerRequest_should_accept_null_reason()
    {
        var validator = new CreateSellerRequestCommandValidator();

        var command = new CreateSellerRequestCommand(
            Guid.NewGuid(),
            null);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void CreateSellerRequest_should_accept_reason_with_1000_characters()
    {
        var validator = new CreateSellerRequestCommandValidator();

        var command = new CreateSellerRequestCommand(
            Guid.NewGuid(),
            new string('a', 1000));

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void CreateSellerRequest_should_reject_reason_longer_than_1000_characters()
    {
        var validator = new CreateSellerRequestCommandValidator();

        var command = new CreateSellerRequestCommand(
            Guid.NewGuid(),
            new string('a', 1001));

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void CreateSellerRequest_should_reject_empty_user_id()
    {
        var validator = new CreateSellerRequestCommandValidator();

        var command = new CreateSellerRequestCommand(Guid.Empty, "I want to become a seller.");
        
        var result = validator.Validate(command);
        
        Assert.That(result.IsValid, Is.False);
    }
    
    [Test]
    public void ApproveSellerRequest_should_accept_valid_command()
    {
        var validator = new ApproveSellerRequestCommandValidator();

        var command = new ApproveSellerRequestCommand(
            Guid.NewGuid());

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void ApproveSellerRequest_should_reject_empty_request_id()
    {
        var validator = new ApproveSellerRequestCommandValidator();

        var command = new ApproveSellerRequestCommand(
            Guid.Empty);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void RejectSellerRequest_should_accept_valid_command()
    {
        var validator = new RejectSellerRequestCommandValidator();

        var command = new RejectSellerRequestCommand(
            Guid.NewGuid());

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void RejectSellerRequest_should_reject_empty_request_id()
    {
        var validator = new RejectSellerRequestCommandValidator();

        var command = new RejectSellerRequestCommand(
            Guid.Empty);

        var result = validator.Validate(command);

        Assert.That(result.IsValid, Is.False);
    }
}