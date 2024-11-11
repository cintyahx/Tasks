using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;

namespace Miotto.Tasks.Infra.Repositories
{
    public class TaskCommentRepository : RepositoryBase<TaskComment>, ITaskCommentRepository
    {
        public TaskCommentRepository(TasksContext tasksContext) : base(tasksContext)
        {
        }

        public async Task<IEnumerable<TaskComment>> GetAllFromTaskAsync(Guid taskId)
        {
            return await Set.Where(x => x.TaskId == taskId && x.IsActive).ToListAsync();
        }
    }
}
