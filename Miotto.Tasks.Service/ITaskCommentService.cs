using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Service
{
    public interface ITaskCommentService
    {
        Task<TaskCommentDto?> GetAsync(Guid id);
        Task<IEnumerable<TaskCommentDto>> GetAllFromTaskAsync(Guid taskId);
        Task<TaskComment> CreateAsync(TaskCommentDto createTaskCommentDto);
        Task<TaskCommentDto?> UpdateAsync(TaskCommentDto updateTaskCommentDto);
        Task DeleteAsync(Guid id);
    }
}
