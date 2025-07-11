using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.EntityTypeConfiguration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
    
            builder.HasMany(c => c.ProjectsAsCustomer)
                .WithOne(p => p.CustomerCompany)
                .HasForeignKey(p => p.CustomerCompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ProjectsAsContractor)
                .WithOne(p => p.ContractorCompany)
                .HasForeignKey(p => p.ContractorCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
