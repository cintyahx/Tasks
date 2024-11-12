using FluentValidation;
using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;

namespace Miotto.Tasks.Service.Validators
{
    public class TaskCommentValidator : AbstractValidator<TaskCommentDto>
    {
        public TaskCommentValidator(IHttpContextAccessor httpContextAccessor,
            IProjectTaskService projectTaskService)
        {
            if (httpContextAccessor!.HttpContext!.Request.Method == HttpMethod.Post.Method)
            {
                RuleFor(x => x.Id)
                .Empty();
            }

            if (httpContextAccessor!.HttpContext!.Request.Method == HttpMethod.Put.Method)
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            }

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.TaskId)
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.TaskId)
                        .MustAsync(async (taskId, _) => await projectTaskService.GetAsync(taskId) != null)
                        .WithMessage("Task not found");
                });

            RuleFor(x => x.User)
                .NotNull()
                .SetValidator(new UserValidator());
        }
    }
}
