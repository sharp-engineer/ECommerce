using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.Application.Abstractions.Identity;
using ECommerce.Infrastructure.Identity;
using Microsoft.Extensions.Options;

namespace ECommerce.Tests.Infrastructure.Identity;

public sealed class JwtTokenServiceTests
{
    [Test]
    public void Generate_should_create_valid_access_token()
    {
        var options = Options.Create(
            new JwtOptions
            {
                SecretKey = new string('t', 32),
                Issuer = "ECommerce.Tests",
                Audience = "ECommerce.Tests",
                ExpirationMinutes = 60
            });

        var service = new JwtTokenService(options);

        var userId = Guid.NewGuid();

        var user = new AuthenticatedUser(
            userId,
            "seller@test.com",
            ["Seller"]);

        var result = service.Generate(user);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result.Token);

        Assert.Multiple(() =>
        {
            Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
            Assert.That(token.Issuer, Is.EqualTo("ECommerce.Tests"));
            Assert.That(
                token.Audiences.Single(),
                Is.EqualTo("ECommerce.Tests"));

            Assert.That(
                token.Subject,
                Is.EqualTo(userId.ToString()));

            Assert.That(
                token.Claims.Single(
                    x => x.Type == JwtRegisteredClaimNames.Email).Value,
                Is.EqualTo("seller@test.com"));

            Assert.That(
                token.Claims.Single(
                    x => x.Type == ClaimTypes.Role).Value,
                Is.EqualTo("Seller"));

            Assert.That(
                result.ExpiresAtUtc,
                Is.EqualTo(token.ValidTo)
                    .Within(TimeSpan.FromSeconds(1)));
        });
    }
}