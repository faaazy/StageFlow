using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Schedule;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.Infrastructure.Schedule;

public class PerformanceRepository(AppDbContext dbContext) : IPerformanceRepository
{
    public async Task<Performance> AddAsync(Performance performance)
    {
        await dbContext.Performances.AddAsync(performance);

        await dbContext.SaveChangesAsync();

        return performance;
    }

    public async Task<List<Performance>> GetAllAsync()
    {
        return await dbContext.Performances
            .Include(p => p.Artist)
            .Include(p => p.Stage)
            .AsNoTracking()
            .ToListAsync();
    }
}