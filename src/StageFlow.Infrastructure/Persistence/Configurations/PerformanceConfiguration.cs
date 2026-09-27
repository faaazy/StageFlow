using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StageFlow.Domain.Festival;

namespace StageFlow.Infrastructure.Persistence.Configurations;

public class PerformanceConfiguration : IEntityTypeConfiguration<Performance>
{
    public void Configure(EntityTypeBuilder<Performance> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Artist)
            .WithMany()
            .HasForeignKey(p => p.ArtistId);

        builder.HasOne(p => p.Stage)
            .WithMany()
            .HasForeignKey(p => p.StageId);
    }
}