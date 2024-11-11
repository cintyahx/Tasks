using Miotto.Tasks.Domain.Enums;

namespace Miotto.Tasks.Domain.Entities
{
    public class ProjectTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }

        public virtual Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
