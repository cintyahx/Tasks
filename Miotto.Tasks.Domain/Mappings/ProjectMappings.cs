using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Domain.Mappings
{
    public static class ProjectMappings
    {
        public static ProjectDto ToDto(this Project project)
        {
            return new ProjectDto
            {
                Title = project.Title,
                User = new UserDto() { Id = project.UserId }
            };
        }

        public static Project ToEntity(this ProjectDto projectDto)
        {
            return new Project
            {
                Title = projectDto.Title,
                UserId = projectDto.User.Id
            };
        }
    }
}
