namespace Miotto.Tasks.Domain.Entities
{
    public class TaskComment : BaseEntity
    {
        public string Description { get; set; }
        public virtual string User { get; set; }

        public Guid TaskId { get; set; }
        public virtual ProjectTask Task { get; set; }
    }
}
