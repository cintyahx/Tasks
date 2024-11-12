using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;
using Miotto.Tasks.Service.Validators;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service.Validators
{
    public class ProjectTaskValidatorTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IProjectService> _projectService;

        private ProjectTaskValidator _projectTaskValidator;

        public ProjectTaskValidatorTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            _projectService = new Mock<IProjectService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Get;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            CreateValidator();
        }

        private void CreateValidator()
        {
            _projectTaskValidator = new ProjectTaskValidator(_httpContextAccessor.Object, _projectService.Object);
        }

        [Fact]
        public async Task Should_Get_Default_Errors()
        {
            var projectTaskDto = new ProjectTaskDto();

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Title' must not be empty.",
                "'Title' must not be empty.",
                "'Description' must not be empty.",
                "'Description' must not be empty.",
                "'Expiration Date' must not be empty.",
                $"'Expiration Date' must be greater than '{DateOnly.FromDateTime(DateTime.Now)}'.",
                "'Project Id' must not be empty.",
                "'User' must not be empty.",
            });
        }

        [Fact]
        public async Task Should_Be_Totally_Valid()
        {
            var projectTaskDto = CreateAValidScenario();

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Validate_When_Project_Not_Found()
        {
            var projectTaskDto = CreateAValidScenario();

            _projectService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(value: null);

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "Project not found",
            });
        }

        [Fact]
        public async Task Should_Validate_When_Project_Not_Allow_New_Tasks()
        {
            var projectTaskDto = CreateAValidScenario();

            _projectService.Setup(x => x.AllowNewTaskAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "Take it easy! There are already too many open tasks for this project.",
            });
        }

        [Fact]
        public async Task Should_Validade_Id_Empty_When_Using_Post_Method()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            var projectTaskDto = CreateAValidScenario();

            CreateValidator();

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Id' must be empty."
            });
        }

        [Fact]
        public async Task Should_Validade_Id_Not_Empty_When_Using_Put_Method()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Put;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            var projectTaskDto = CreateAValidScenario();
            projectTaskDto.Id = Guid.Empty;
            CreateValidator();

            var validation = await _projectTaskValidator.ValidateAsync(projectTaskDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Id' must not be empty."
            });
        }


        private ProjectTaskDto CreateAValidScenario()
        {
            var project = _fixture.Create<ProjectDto>();
            var user = _fixture.Create<UserDto>();

            var projectTaskDto = _fixture.Create<ProjectTaskDto>();
            projectTaskDto.ProjectId = project.Id;
            projectTaskDto.ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            projectTaskDto.User = user;

            _projectService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(project);

            _projectService.Setup(x => x.AllowNewTaskAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            return projectTaskDto;
        }
    }
}
