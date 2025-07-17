using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(u => u.Email)
                .HasMaxLength(256);
            builder.Property(u => u.Name)
                .HasMaxLength(100);
            builder.Property(u => u.LastName)
                .HasMaxLength(100);
            builder.Property(u => u.Patronymic)
                .HasMaxLength(100);
            builder.HasMany(u => u.ProjectUsers)
                .WithOne(pu => pu.User)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u=>u.TaskUsers)
                .WithOne(tu=>tu.User)
                .HasForeignKey(tu => tu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.ProjectsAsLeader)
                .WithOne(p => p.Leader)
                .HasForeignKey(p => p.LeaderUserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
