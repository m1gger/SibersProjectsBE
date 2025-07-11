
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.EntityTypeConfiguration
{
    public class ProjectDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
    {
        public void Configure(EntityTypeBuilder<ProjectDocument> builder)
        {
            builder.HasKey(pd => pd.Id);
            builder.Property(pd => pd.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(pd => pd.Description)
                .HasMaxLength(200);
            
            builder.Property(pd => pd.Path)
                .IsRequired()
                .HasMaxLength(1024);
            builder.HasOne(pd => pd.Project)
                .WithMany(p => p.ProjectDocuments)
                .HasForeignKey(pd => pd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
