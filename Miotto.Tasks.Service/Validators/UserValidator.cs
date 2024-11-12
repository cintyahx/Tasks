using FluentValidation;
using Miotto.Tasks.Domain.Dtos;

namespace Miotto.Tasks.Service.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Man, at least tell me who you are.");
        }
    }
}
