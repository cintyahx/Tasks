namespace Miotto.Tasks.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public virtual IEnumerable<ProjectTask> Tasks { get; set; }
    }
}
