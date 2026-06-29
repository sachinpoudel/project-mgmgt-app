using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;

public class ProjectConfiguration: IEntityTypeConfiguration<Project>
{
        
        public void Configure(EntityTypeBuilder<Project> builder)
    {
        
        builder.ToTable("Projects"); 
        builder.HasKey( p => p.Id);
        builder.Property(p => p.ProjectName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Status).HasMaxLength(50).HasConversion<string>();
        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate).IsRequired();
        builder.Property(p => p.OwnerId).IsRequired();

        builder.HasMany(p => p.Members).WithOne(pm => pm.Project).HasForeignKey(pm => pm.ProjectId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Tasks).WithOne(t => t.Project).HasForeignKey(pt => pt.ProjectId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Owner).WithMany().HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Restrict);
    }

       
}