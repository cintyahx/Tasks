namespace Miotto.Tasks.Domain.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public UserDto User { get; set; }
    }
}
