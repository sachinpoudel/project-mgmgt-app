using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;


public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(tc => tc.Id);
        builder.Property(tc => tc.TaskId).IsRequired();
        builder.Property(tc => tc.UserId).IsRequired();
        builder.Property(tc => tc.CommentText).IsRequired().HasMaxLength(1000);
        builder.Property(tc => tc.dateTime).IsRequired();

        builder.HasOne(tc => tc.Task).WithMany(t => t.Comments).HasForeignKey(tc => tc.TaskId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tc => tc.User).WithMany(u => u.TaskComments).HasForeignKey(tc => tc.UserId).OnDelete(DeleteBehavior.Restrict);
    } 

    
}