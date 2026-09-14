using Microsoft.EntityFrameworkCore;
using StageFlow.Domain.Festival;

namespace StageFlow.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Festival> Festivals {get; set;}
} 