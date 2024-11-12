using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Infra.Mappings
{
    public class TaskCommentMapping : IEntityTypeConfiguration<TaskComment>
    {
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.TaskId).IsRequired();
            builder.HasOne(x => x.Task)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.TaskId);
        }
    }
}
