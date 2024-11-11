namespace Miotto.Tasks.Domain.Dtos
{
    public class TaskCommentDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public Guid TaskId { get; set; }
    }
}
