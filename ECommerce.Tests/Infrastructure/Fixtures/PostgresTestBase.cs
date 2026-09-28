using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace ECommerce.Tests.Infrastructure.Fixtures;

public abstract class PostgresTestBase
{
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder("postgres:15.1")
            .WithDatabase("ecommerce_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    protected DbContextOptions<ECommerceDbContext> DbContextOptions { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task StartDatabase()
    {
        await _postgresContainer.StartAsync();

        DbContextOptions = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options;

        await using var dbContext = new ECommerceDbContext(DbContextOptions);

        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task StopDatabase()
    {
        await _postgresContainer.DisposeAsync();
    }

    protected ECommerceDbContext CreateDbContext()
        => new(DbContextOptions);

    protected string ConnectionString =>
        _postgresContainer.GetConnectionString();
}