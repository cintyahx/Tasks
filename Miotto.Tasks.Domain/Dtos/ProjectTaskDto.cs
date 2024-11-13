using Miotto.Tasks.Domain.Enums;

namespace Miotto.Tasks.Domain.Dtos
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Guid ProjectId { get; set; }

        public UserDto User { get; set; }
    }
}
