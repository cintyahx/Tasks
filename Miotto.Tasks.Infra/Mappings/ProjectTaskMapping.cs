using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Infra.Mappings
{
    public class ProjectTaskMapping : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.ExpirationDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.ProjectId).IsRequired();
            builder.HasOne(x => x.Project)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.ProjectId);
        }
    }
}
