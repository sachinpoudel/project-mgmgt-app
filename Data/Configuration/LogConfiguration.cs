using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;

public class LogConfiguration : IEntityTypeConfiguration<TimeLog>
{
    public void Configure(EntityTypeBuilder<TimeLog> builder)
    {
        builder.HasKey(tl => tl.Id);
        builder.Property(tl => tl.TaskId).IsRequired();
        builder.Property(tl => tl.UserId).IsRequired();
        builder.Property(tl => tl.LogDate).IsRequired();
        builder.Property(tl => tl.Description).HasMaxLength(1000);

        builder.HasOne(tl => tl.Task).WithMany(t => t.TimeLogs).HasForeignKey(tl => tl.TaskId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tl => tl.User).WithMany(u => u.TimeLogs).HasForeignKey(tl => tl.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}