using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StageFlow.Application.Auth;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Identity;
using StageFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class IdentityRegistrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("stageflow_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private AppDbContext _dbContext = null!;

    private ServiceProvider _serviceProvider = null!;
    private IServiceScope _scope = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = _postgres.GetConnectionString();
            options.UseNpgsql(connectionString);
        });

        serviceCollection.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>();

        serviceCollection.AddScoped<IUserAccountCreator, IdentityUserAccountCreator>();

        _serviceProvider = serviceCollection.BuildServiceProvider();

        _scope = _serviceProvider.CreateScope();

        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await _dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _serviceProvider.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_CreatesUser()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();

        var result = await creator.CreateAsync("mockEmail@example.com", "mockPassword123!");

        Assert.True(result.Succeeded);

        var user = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.Email == "mockEmail@example.com");

        user.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_WithExistingEmail_ReturnsFailure()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();

        var result = await creator.CreateAsync("mockEmail@example.com", "mockPassword123!");

        Assert.True(result.Succeeded);

        var user = await _dbContext.Users.SingleOrDefaultAsync(
            u => u.Email == "mockEmail@example.com");

        user.Should().NotBeNull();

        var result2 = await creator.CreateAsync("mockEmail@example.com", "mockPassword123!");

        Assert.False(result2.Succeeded);
    }

    [Fact]
    public async Task CreateAsync_WithWeakPassword_ReturnsFailure()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();

        var result = await creator.CreateAsync("weak-password@example.com", "abc");

        Assert.False(result.Succeeded);

        result.Errors.Should().NotBeEmpty();
    }
}
