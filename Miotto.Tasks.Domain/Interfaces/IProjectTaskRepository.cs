using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Enums;

namespace Miotto.Tasks.Domain.Interfaces
{
    public interface IProjectTaskRepository : IRepository<ProjectTask>
    {
        Task<IEnumerable<ProjectTask>> GetAllFromProjectAsync(Guid projectId);
        Task<IEnumerable<ProjectTask>> GetAllFromProjectAsync(Guid projectId, Status[] status);
        Task<IEnumerable<ProjectTask>> GetTasksDoneLastMonthAsync(Guid userId);
    }
}
