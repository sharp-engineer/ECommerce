using ECommerce.Tests.Infrastructure.Fixtures;

namespace ECommerce.Tests.Infrastructure.Persistence;

public sealed class ECommerceDbContextTests : PostgresTestBase
{
    [Test]
    public async Task Can_connect_to_database()
    {
        await using var dbContext = CreateDbContext();

        var canConnect = await dbContext.Database.CanConnectAsync();

        Assert.That(canConnect, Is.True);
    }
}