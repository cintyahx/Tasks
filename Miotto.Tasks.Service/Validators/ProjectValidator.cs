using FluentValidation;
using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;

namespace Miotto.Tasks.Service.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator(IHttpContextAccessor httpContextAccessor, 
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

            if (httpContextAccessor!.HttpContext!.Request.Method == HttpMethod.Delete.Method)
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Id)
                            .MustAsync(async (id, _) => await projectService.AllowDeleteAsync(id))
                            .WithMessage("There are open tasks for this project. Remove or complete them before deleting.");
                    });
            }

            RuleFor(x => x.User)
                .NotNull()
                .SetValidator(new UserValidator());
        }
    }
}
