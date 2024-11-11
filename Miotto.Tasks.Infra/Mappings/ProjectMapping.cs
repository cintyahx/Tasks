using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Infra.Mappings
{
    public class ProjectMapping : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Title);

            builder.Property(x => x.Owner).IsRequired();
        }
    }
}
