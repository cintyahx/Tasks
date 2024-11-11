using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;

namespace Miotto.Tasks.Infra.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(TasksContext tasksContext) : base(tasksContext)
        {
        }
    }
}
