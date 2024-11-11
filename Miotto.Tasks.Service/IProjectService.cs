using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Service
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(ProjectDto createProjectDto);
        Task<ProjectDto?> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<bool> AllowDeleteAsync(Guid id);
        Task<bool> AllowNewTaskAsync(Guid id);
    }
}
