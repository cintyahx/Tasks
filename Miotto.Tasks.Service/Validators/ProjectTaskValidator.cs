using FluentValidation;
using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;

namespace Miotto.Tasks.Service.Validators
{
    public class ProjectTaskValidator : AbstractValidator<ProjectTaskDto>
    {
        public ProjectTaskValidator(IHttpContextAccessor httpContextAccessor, 
            IProjectService projectService)
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

            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ExpirationDate)
                .NotNull()
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now));

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .DependentRules(() => {
                    RuleFor(x => x.ProjectId)
                        .MustAsync(async (projectId, _) => await projectService.GetAsync(projectId) != null)
                        .WithMessage("Project not found")
                        .DependentRules(() => {
                            RuleFor(x => x.ProjectId)
                                .MustAsync(async (projectId, _) => await projectService.AllowNewTaskAsync(projectId))
                                .WithMessage("Take it easy! There are already too many open tasks for this project.");
                        });
                });

            RuleFor(x => x.User)
                .NotNull()
                .SetValidator(new UserValidator());
        }
    }
}
