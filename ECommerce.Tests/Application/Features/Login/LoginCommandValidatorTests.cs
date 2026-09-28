using ECommerce.Application.Features.Authentication.Login;

namespace ECommerce.Tests.Application.Features.Login;

public sealed class LoginCommandValidatorTests
{
    [Test]
    public void LoginValidator_should_accept_valid_command()
    {
        var validator = new LoginCommandValidator();

        var result = validator.Validate(
            new LoginCommand(
                "seller@test.com",
                "Password123!"));

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void LoginValidator_should_reject_invalid_email()
    {
        var validator = new LoginCommandValidator();

        var result = validator.Validate(
            new LoginCommand(
                "invalid-email",
                "Password123!"));

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void LoginValidator_should_reject_empty_password()
    {
        var validator = new LoginCommandValidator();

        var result = validator.Validate(
            new LoginCommand(
                "seller@test.com",
                ""));

        Assert.That(result.IsValid, Is.False);
    }
}