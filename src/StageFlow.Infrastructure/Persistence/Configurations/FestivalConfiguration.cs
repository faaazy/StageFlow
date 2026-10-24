using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Identity;

namespace StageFlow.Infrastructure.Persistence.Configurations;

public class FestivalConfiguration : IEntityTypeConfiguration<Festival>
{
    public void Configure(EntityTypeBuilder<Festival> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(f => f.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}