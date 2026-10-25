using StageFlow.Infrastructure.Identity;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.IntegrationTests.Infrastructure;

public static class TestData
{
    public static async Task<Guid> CreateUserAsync(AppDbContext dbContext)
    {
        var id = Guid.NewGuid();

        dbContext.Users.Add(new ApplicationUser
        {
            Id = id,
            UserName = $"user-{id:N}",
            NormalizedUserName = $"USER-{id:N}".ToUpperInvariant(),
            Email = $"user-{id:N}@example.com"
        });

        await dbContext.SaveChangesAsync();

        return id;
    }
}