using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;

namespace Miotto.Tasks.Service
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _taskProjectRepository;

        public ProjectTaskService(IProjectTaskRepository taskProjectRepository)
        {
            _taskProjectRepository = taskProjectRepository;
        }

        public async Task<ProjectTask> CreateAsync(ProjectTaskDto createTaskProjectDto)
        {
            var task = createTaskProjectDto.ToEntity();
            return await _taskProjectRepository.CreateAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _taskProjectRepository.GetAsync(id);

            if (entity != null)
            {
                await _taskProjectRepository.DeleteAsync(entity);
            }
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetAllFromProjectAsync(Guid projectId)
        {
            var tasks = await _taskProjectRepository.GetAllFromProjectAsync(projectId);
            return tasks.Select(task => task.ToDto());
        }

        public async Task<ProjectTaskDto?> GetAsync(Guid id)
        {
            var task = await _taskProjectRepository.GetAsync(id);
            return task?.ToDto();
        }

        public async Task<ProjectTaskDto?> UpdateAsync(ProjectTaskDto updateTaskProjectDto)
        {
            var task = await _taskProjectRepository.GetAsync(updateTaskProjectDto.Id);
            if (task == null)
            {
                return null;
            }

            UpdateProperties(updateTaskProjectDto, task!);

            await _taskProjectRepository.UpdateAsync(task!);

            return task!.ToDto();
        }

        private static void UpdateProperties(ProjectTaskDto updateTaskProjectDto, ProjectTask task)
        {
            task.Title = updateTaskProjectDto.Title;
            task.Description = updateTaskProjectDto.Description;
            task.ExpirationDate = updateTaskProjectDto.ExpirationDate;
            task.Status = updateTaskProjectDto.Status;
            task.ProjectId = updateTaskProjectDto.ProjectId;
        }
    }
}
