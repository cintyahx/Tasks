using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service.Validators;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service.Validators
{
    public class UserValidatorTests
    {
        private readonly Fixture _fixture;

        private UserValidator _userValidator;

        public UserValidatorTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            CreateValidator();
        }

        private void CreateValidator()
        {
            _userValidator = new UserValidator();
        }

        [Fact]
        public async Task Should_Be_Totally_Valid()
        {
            var userDto = CreateAValidScenario();

            var validation = await _userValidator.ValidateAsync(userDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Validate_Name_Empty()
        {
            var userDto = new UserDto();
            userDto.Id = Guid.Empty;

            var validation = await _userValidator.ValidateAsync(userDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "Man, at least tell me who you are.",
            });
        }

        private UserDto CreateAValidScenario()
        {
            return _fixture.Create<UserDto>();
        }
    }
}
