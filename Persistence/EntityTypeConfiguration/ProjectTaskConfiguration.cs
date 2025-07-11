using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.EntityTypeConfiguration
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder) 
        {
            builder.HasKey(builder => builder.Id);
            builder.Property(builder => builder.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(builder => builder.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(builder => builder.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasOne(pt => pt.Project)
                 .WithMany(p => p.Tasks)
                 .HasForeignKey(pt => pt.ProjectId)
                 .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pt=>pt.Leader)
                .WithMany(p => p.LeaderTasks)
                .HasForeignKey(pt => pt.LeaderUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pt=>pt.AssignedUser)
                .WithMany(p=>p.AssignedTasks)
                .HasForeignKey(pt => pt.AssignedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
