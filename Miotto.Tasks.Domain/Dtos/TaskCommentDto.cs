namespace Miotto.Tasks.Domain.Dtos
{
    public class TaskCommentDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid TaskId { get; set; }
        public UserDto User { get; set; }
    }
}
