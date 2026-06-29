using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mvc_dotnet.Models;

namespace mvc_dotnet.Data.Configuration;
public class ProjectTaskConfiguration: IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("ProjectTasks");
        builder.HasKey(pt => pt.Id);
        builder.Property( pt => pt.Title).IsRequired().HasMaxLength(200);
        builder.Property(pt => pt.Description).HasMaxLength(1000);
        builder.Property(pt => pt.Status).IsRequired().HasMaxLength(50);
        builder.Property(pt => pt.Priority).HasMaxLength(50);
        builder.Property(pt => pt.DueDate).IsRequired();
        builder.Property(pt => pt.EstimatedHours).HasColumnType("decimal(18,2)");
        builder.Property(pt => pt.CreatedById).IsRequired();
        builder.Property(pt => pt.AssignedToId);
        builder.Property(pt => pt.ParentTaskId);
        builder.Property(pt => pt.Tags).HasMaxLength(200);
     
    
            builder.HasMany(pt => pt.SubTasks).WithOne(sb => sb.ParentTask).HasForeignKey(sb => sb.ParentTaskId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pt => pt.Attachments).WithOne(ta => ta.Task).HasForeignKey(t => t.TaskId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pt => pt.TimeLogs).WithOne(tl => tl.Task).HasForeignKey(tl => tl.TaskId).OnDelete(DeleteBehavior.Restrict);
        
    
}}