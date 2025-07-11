using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.EntityTypeConfiguration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(p => p.StartDate)
                .IsRequired();
            builder.Property(p => p.EndDate)
                .IsRequired();
            builder.HasMany(p => p.ProjectUsers)
                .WithOne(pu => pu.Project)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u=>u.Leader)
                .WithMany(p=>p.ProjectsAsLeader)
                .HasForeignKey(p => p.LeaderUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p=>p.ProjectCompanies)
                .WithOne(pc=>pc.Project)
                .HasForeignKey(pc => pc.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
