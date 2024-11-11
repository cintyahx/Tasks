using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Domain.Interfaces
{
    public interface ITaskCommentRepository : IRepository<TaskComment>
    {
        Task<IEnumerable<TaskComment>> GetAllFromTaskAsync(Guid taskId);
    }
}
