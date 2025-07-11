using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.EntityTypeConfiguration
{
    class UserTaskConfiguration : IEntityTypeConfiguration<TaskUser>
    {
        public void Configure(EntityTypeBuilder<TaskUser> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.TaskId });
            builder.HasOne(ut => ut.User)
                .WithMany(u => u.TaskUsers)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ut => ut.Task)
                .WithMany(t => t.TaskUsers)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
