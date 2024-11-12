using Microsoft.AspNetCore.Http;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service.Validators;
using Miotto.Tasks.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miotto.Tasks.Tests.Service.Validators
{
    public class TaskCommentValidatorTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IProjectTaskService> _projectTaskService;

        private TaskCommentValidator _taskCommentValidator;

        public TaskCommentValidatorTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            _projectTaskService = new Mock<IProjectTaskService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Get;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            CreateValidator();
        }

        private void CreateValidator()
        {
            _taskCommentValidator = new TaskCommentValidator(_httpContextAccessor.Object, _projectTaskService.Object);
        }

        [Fact]
        public async Task Should_Get_Default_Errors()
        {
            var commentDto = new TaskCommentDto();

            var validation = await _taskCommentValidator.ValidateAsync(commentDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Description' must not be empty.",
                "'Description' must not be empty.",
                "'Task Id' must not be empty.",
                "'User' must not be empty.",
            });
        }

        [Fact]
        public async Task Should_Be_Totally_Valid()
        {
            var commentDto = CreateAValidScenario();

            var validation = await _taskCommentValidator.ValidateAsync(commentDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Validate_When_Project_Not_Found()
        {
            var commentDto = CreateAValidScenario();

            _projectTaskService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(value: null);

            var validation = await _taskCommentValidator.ValidateAsync(commentDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "Task not found",
            });
        }

        [Fact]
        public async Task Should_Validade_Id_Empty_When_Using_Post_Method()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethods.Post;

            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            var commentDto = CreateAValidScenario();

            CreateValidator();

            var validation = await _taskCommentValidator.ValidateAsync(commentDto);

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

            var commentDto = CreateAValidScenario();
            commentDto.Id = Guid.Empty;
            CreateValidator();

            var validation = await _taskCommentValidator.ValidateAsync(commentDto);

            validation.Errors.Select(x => x.ErrorMessage).Should().BeEquivalentTo(new string[]{
                "'Id' must not be empty."
            });
        }

        private TaskCommentDto CreateAValidScenario()
        {
            var task = _fixture.Create<ProjectTaskDto>();
            var user = _fixture.Create<UserDto>();

            var commentDto = _fixture.Create<TaskCommentDto>();
            commentDto.TaskId = task.Id;
            commentDto.User = user;

            _projectTaskService.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(task);

            return commentDto;
        }
    }
}
