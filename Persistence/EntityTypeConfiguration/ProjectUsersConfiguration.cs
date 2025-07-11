using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    public class ProjectUsersConfiguration : IEntityTypeConfiguration<ProjectUsers>
    {
        public void Configure(EntityTypeBuilder<ProjectUsers> builder)
        {
            builder.HasKey(pu => new { pu.ProjectId, pu.UserId });
            builder.HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
