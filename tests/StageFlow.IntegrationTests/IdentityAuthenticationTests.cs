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

public class IdentityAuthenticationTests : IAsyncLifetime
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
        serviceCollection.AddScoped<IUserAuthenticator, IdentityUserAuthenticator>();

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
    public async Task Authenticate_WithCorrectPassword()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();
        var authenticator = _scope.ServiceProvider.GetRequiredService<IUserAuthenticator>();

        var email = "mockEmail@example.com";
        var password =  "mockPassword123!";

        var createdUser = await creator.CreateAsync(email, password);

        var result = await authenticator.AuthenticateAsync(email, password);

        Assert.True(result.Succeeded);
        Assert.Equal(result.UserId, createdUser.UserId);
    }
    
    [Fact]
    public async Task Authenticate_WithIncorrectPassword_ReturnsFailure()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();
        var authenticator = _scope.ServiceProvider.GetRequiredService<IUserAuthenticator>();

        var email = "newEmail@example.com";
        var password =  "newPassword123!";

        var wrongPassword = "wrongPassword123!";

        await creator.CreateAsync(email, password);

        var result = await authenticator.AuthenticateAsync(email, wrongPassword);

        Assert.False(result.Succeeded);
        Assert.Null(result.UserId);
    }

    [Fact]
    public async Task Authenticate_WithIncorrectEmail_ReturnsFailure()
    {
        var creator = _scope.ServiceProvider.GetRequiredService<IUserAccountCreator>();
        var authenticator = _scope.ServiceProvider.GetRequiredService<IUserAuthenticator>();

        var email = "newEmail2@example.com";
        var password =  "newPassword2123!";

        var wrongEmail = "wrongEmail@example.com";

        await creator.CreateAsync(email, password);

        var result = await authenticator.AuthenticateAsync(wrongEmail, password);

        Assert.False(result.Succeeded);
        Assert.Null(result.UserId);
    }
}
