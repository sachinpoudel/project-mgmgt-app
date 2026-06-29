using Microsoft.EntityFrameworkCore;
using mvc_dotnet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mvc_dotnet.Data.Configuration;


public class ProjectMemeberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.HasKey(m => new {m.UserId, m.ProjectId}); // composite key to ensure uniqueness of user in a project
        builder.Property(pm => pm.ProjectId).IsRequired();
        builder.Property(pm => pm.UserId).IsRequired();
        builder.Property(pm => pm.Role).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(pm => pm.JoinedAt).IsRequired();


        builder.HasOne(pm => pm.User).WithMany(u => u.ProjectMembers).HasForeignKey(pm => pm.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pm => pm.Project).WithMany(p => p.Members).HasForeignKey(pm => pm.ProjectId).OnDelete(DeleteBehavior.Restrict);

       

    }
}