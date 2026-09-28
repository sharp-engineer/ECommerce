using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class ECommerceDbContextTests
{
    [Test]
    public async Task Can_connect_to_database()
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_TEST_CONNECTION_STRING");
        Assert.That(connectionString, Is.Not.Null.And.Not.Empty);

        var options = new DbContextOptionsBuilder<ECommerceDbContext>().UseNpgsql(connectionString).Options;
        await using var dbContext = new ECommerceDbContext(options);
        
        var canConnect = await dbContext.Database.CanConnectAsync();
        Assert.That(canConnect, Is.True);
    }
}