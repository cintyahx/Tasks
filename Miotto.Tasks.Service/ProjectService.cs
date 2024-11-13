using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Enums;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;

namespace Miotto.Tasks.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;

        public ProjectService(IProjectRepository projectRepository,
            IProjectTaskRepository projectTaskRepository)
        {
            _projectRepository = projectRepository;
            _projectTaskRepository = projectTaskRepository;
        }

        public async Task<bool> AllowDeleteAsync(Guid id)
        {
            var tasks = await _projectTaskRepository.GetAllFromProjectAsync(id, [Status.Open, Status.Doing]);
            return !tasks.Any();
        }

        public async Task<bool> AllowNewTaskAsync(Guid id)
        {
            var tasks = await _projectTaskRepository.GetAllFromProjectAsync(id, [Status.Open, Status.Doing]);
            return tasks.Count() < 20;
        }

        public async Task<Project> CreateAsync(ProjectDto createProjectDto)
        {
            var project = createProjectDto.ToEntity();
            return await _projectRepository.CreateAsync(project);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _projectRepository.GetAsync(id);
            if (entity != null)
            {
                await _projectRepository.DeleteAsync(entity);
            }
        }

        public async Task<ProjectDto?> GetAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);
            return project?.ToDto();
        }
    }
}
