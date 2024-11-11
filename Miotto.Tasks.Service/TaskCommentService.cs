using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;

namespace Miotto.Tasks.Service
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository _taskCommentRepository;

        public TaskCommentService(ITaskCommentRepository taskCommentRepository)
        {
            _taskCommentRepository = taskCommentRepository;
        }

        public async Task<TaskComment> CreateAsync(TaskCommentDto createTaskCommentDto)
        {
            var comment = createTaskCommentDto.ToEntity();
            return await _taskCommentRepository.CreateAsync(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _taskCommentRepository.GetAsync(id);

            if (entity != null)
            {
                await _taskCommentRepository.DeleteAsync(entity);
            }
        }

        public async Task<IEnumerable<TaskCommentDto>> GetAllFromTaskAsync(Guid taskId)
        {
            var comments = await _taskCommentRepository.GetAllFromTaskAsync(taskId);
            return comments.Select(task => task.ToDto());
        }

        public async Task<TaskCommentDto?> GetAsync(Guid id)
        {
            var comment = await _taskCommentRepository.GetAsync(id);
            return comment?.ToDto();
        }

        public async Task<TaskCommentDto?> UpdateAsync(TaskCommentDto updateTaskCommentDto)
        {
            var comment = await _taskCommentRepository.GetAsync(updateTaskCommentDto.Id);
            if (comment == null)
            {
                return null;
            }

            UpdateProperties(updateTaskCommentDto, comment!);

            await _taskCommentRepository.UpdateAsync(comment!);

            return comment!.ToDto();
        }

        private void UpdateProperties(TaskCommentDto updateTaskCommentDto, TaskComment comment)
        {
            comment.Description = updateTaskCommentDto.Description;
            comment.TaskId = updateTaskCommentDto.TaskId;
        }
    }
}
