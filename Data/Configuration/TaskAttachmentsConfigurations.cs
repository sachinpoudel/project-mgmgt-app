using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;

public class TaskAttachmentsConfigurations : IEntityTypeConfiguration<TaskAttachment>
{
    public void Configure(EntityTypeBuilder<TaskAttachment> builder)
    {
        builder.HasKey(ta => ta.Id);
        builder.Property(ta => ta.TaskId).IsRequired();
        builder.Property(ta => ta.FileName).IsRequired().HasMaxLength(255);
        builder.Property(ta => ta.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(ta => ta.FileType).IsRequired().HasMaxLength(100);
        builder.Property(ta => ta.FileSize).IsRequired().HasMaxLength(50);
        builder.Property(ta => ta.UploadedById).IsRequired();
        builder.Property(ta => ta.UploadedAt).IsRequired();

        builder.HasOne(ta => ta.Task).WithMany(t => t.Attachments).HasForeignKey(ta => ta.TaskId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ta => ta.UploadedBy).WithMany(u => u.TaskAttachments).HasForeignKey(ta => ta.UploadedById).OnDelete(DeleteBehavior.Restrict);
    }
}