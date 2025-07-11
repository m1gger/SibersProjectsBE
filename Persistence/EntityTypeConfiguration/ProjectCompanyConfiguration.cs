
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.EntityTypeConfiguration
{
    public class ProjectCompanyConfiguration : IEntityTypeConfiguration<ProjectCompany>
    {
        public void Configure(EntityTypeBuilder<ProjectCompany> builder)
        {
            builder.HasKey(pc => new { pc.ProjectId, pc.CompanyId });
            builder.HasOne(pc => pc.Project)
                .WithMany(p => p.ProjectCompanies)
                .HasForeignKey(pc => pc.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pc => pc.Company)
                .WithMany(c => c.ProjectCompanies)
                .HasForeignKey(pc => pc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
