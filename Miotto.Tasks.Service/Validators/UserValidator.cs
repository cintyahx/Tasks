using FluentValidation;
using Miotto.Tasks.Domain.Dtos;

namespace Miotto.Tasks.Service.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .DependentRules(() => {
                    RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithMessage("Man, at least tell me who you are.");
                });
        }
    }
}
