using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Service
{
    public interface IProjectTaskService
    {
        Task<IEnumerable<ProjectTaskDto>> GetAllFromProjectAsync(Guid projectId);
        Task<IEnumerable<ProjectTaskDto>> GetTasksDoneLastMonthAsync(Guid userId);
        Task<ProjectTaskDto?> GetAsync(Guid id);
        Task<ProjectTask> CreateAsync(ProjectTaskDto createTaskProjectDto);
        Task<ProjectTaskDto?> UpdateAsync(ProjectTaskDto updateTaskProjectDto);
        Task DeleteAsync(Guid id);
    }
}
