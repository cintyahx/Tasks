using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Domain.Mappings
{
    public static class ProjectMappings
    {
        public static ProjectDto ToDto(this Project task)
        {
            return new ProjectDto
            {
                Title = task.Title
            };
        }

        public static Project ToEntity(this ProjectDto taskDto)
        {
            return new Project
            {
                Title = taskDto.Title
            };
        }
    }
}
