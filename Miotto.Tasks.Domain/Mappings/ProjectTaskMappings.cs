using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Domain.Mappings
{
    public static class ProjectTaskMappings
    {
        public static ProjectTaskDto ToDto(this ProjectTask task)
        {
            return new ProjectTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                ExpirationDate = task.ExpirationDate,
                FinishDate = task.FinishDate,
                Status = task.Status,
                Priority = task.Priority,
                User = new UserDto() { Id = task.UserId }
            };
        }

        public static ProjectTask ToEntity(this ProjectTaskDto taskDto)
        {
            return new ProjectTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                ExpirationDate = taskDto.ExpirationDate,
                FinishDate = taskDto.FinishDate,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                UserId = taskDto.User.Id
            };
        }
    }
}
