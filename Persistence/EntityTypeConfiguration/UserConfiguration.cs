using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Patronymic)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasMany(u => u.ProjectUsers)
                .WithOne(pu => pu.User)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.AssignedTasks)
              .WithOne(p => p.AssignedUser)
              .HasForeignKey(p => p.AssignedUserId)
              .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.LeaderTasks)
                .WithOne(p => p.Leader)
                .HasForeignKey(p => p.LeaderUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
