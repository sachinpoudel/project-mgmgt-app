using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100); 
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property( u => u.PasswordHash).IsRequired();
        builder.Property(u => u.PasswordSalt).IsRequired();
        builder.Property(u => u.ProfileUrl).HasMaxLength(500);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(u => u.LastLogin).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();



     
          builder.HasMany(u => u.ProjectMembers).WithOne(pm => pm.User).HasForeignKey(pm => pm.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ProjectTasks).WithOne(pt => pt.CreatedBy).HasForeignKey(pt => pt.CreatedById).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.AssignedTasks).WithOne(pt => pt.AssignedTo).HasForeignKey(pt => pt.AssignedToId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.TaskComments).WithOne(tc => tc.User).HasForeignKey(tc => tc.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.TaskAttachments).WithOne(ta => ta.UploadedBy).HasForeignKey(ta => ta.UploadedById).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.TimeLogs).WithOne(tl => tl.User).HasForeignKey(tl => tl.UserId).OnDelete(DeleteBehavior.Restrict);


    }
}