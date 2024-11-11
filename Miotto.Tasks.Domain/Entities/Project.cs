namespace Miotto.Tasks.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public virtual IEnumerable<ProjectTask> Tasks { get; set; }
        public virtual string Owner { get; set; }
    }
}
