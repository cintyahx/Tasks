using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Enums;
using Miotto.Tasks.Domain.Interfaces;

namespace Miotto.Tasks.Infra.Repositories
{
    public class ProjectTaskRepository : RepositoryBase<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(TasksContext tasksContext) : base(tasksContext)
        {
        }

        public async Task<IEnumerable<ProjectTask>> GetAllFromProjectAsync(Guid projectId)
        {
            return await GetAllFromProjectAsync(projectId, Enum.GetValues<Status>());
        }

        public async Task<IEnumerable<ProjectTask>> GetAllFromProjectAsync(Guid projectId, Status[] status)
        {
            return await Set.Where(x => x.ProjectId == projectId && x.IsActive && status.Contains(x.Status)).ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksDoneLastMonthAsync(Guid userId)
        {
            return await Set.Where(x => x.UserId == userId
                                        && x.IsActive
                                        && x.Status == Status.Done
                                        && DateTime.Now.Subtract(x.FinishDate.GetValueOrDefault()).Days > 0
                                        && DateTime.Now.Subtract(x.FinishDate.GetValueOrDefault()).Days <= 30)
                .ToListAsync();
        }
    }
}
