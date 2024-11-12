using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;

namespace Miotto.Tasks.Domain.Mappings
{
    public static class TaskCommentMappings
    {
        public static TaskCommentDto ToDto(this TaskComment comment)
        {
            return new TaskCommentDto
            {
                Description = comment.Description,
                TaskId = comment.TaskId,
                User = new UserDto() { Name = comment.User }
            };
        }

        public static TaskComment ToEntity(this TaskCommentDto commentDto)
        {
            return new TaskComment
            {
                Description = commentDto.Description,
                TaskId = commentDto.TaskId,
                User = commentDto.User.Name
            };
        }
    }
}
