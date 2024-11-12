using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;
using Miotto.Tasks.Service.Validators;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service.Validators
{
    public class ProjectValidatorTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IProjectService> _projectService;

        private ProjectValidator _projectValidator;

        public ProjectValidatorTests()
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
            _projectValidator = new ProjectValidator(_httpContextAccessor.Object, _projectService.Object);
        }

        [Fact]
        public async Task Should_Be_Totally_Valid()
        {
            var projectDto = CreateAValidScenario();

            var validation = await _projectValidator.ValidateAsync(projectDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Get_Default_Errors()
        {
            var projectDto = new ProjectDto();

            var validation = await _projectValidator.ValidateAsync(projectDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'User' must not be empty.",
            });
        }

        [Fact]
        public async Task Should_Validade_Id_Empty_When_Using_Post_Method()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            var projectDto = CreateAValidScenario();

            CreateValidator();

            var validation = await _projectValidator.ValidateAsync(projectDto);

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

            var projectDto = CreateAValidScenario();
            projectDto.Id = Guid.Empty;
            CreateValidator();

            var validation = await _projectValidator.ValidateAsync(projectDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Id' must not be empty."
            });
        }

        [Fact]
        public async Task Should_Validade_Id_Not_Empty_When_Using_Delete_Method()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Delete;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            var projectDto = CreateAValidScenario();
            projectDto.Id = Guid.Empty;
            CreateValidator();

            var validation = await _projectValidator.ValidateAsync(projectDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Id' must not be empty."
            });
        }

        [Fact]
        public async Task Should_Validate_When_Project_Not_Allow_Delete()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Delete;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);
            CreateValidator();

            var projectDto = CreateAValidScenario();
            _projectService.Setup(x => x.AllowDeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var validation = await _projectValidator.ValidateAsync(projectDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "There are open tasks for this project. Remove or complete them before deleting.",
            });
        }

        private ProjectDto CreateAValidScenario()
        {
            var project = _fixture.Create<ProjectDto>();

            _projectService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(project);

            _projectService.Setup(x => x.AllowNewTaskAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            return project;
        }
    }
}
