using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Enums;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;
using Miotto.Tasks.Service;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service
{
    public class ProjectServiceTests
    {
        private readonly Fixture _fixture;

        private readonly IProjectService _taskCommentService;

        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IProjectTaskRepository> _projectTaskRepository;

        public ProjectServiceTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            _projectRepository = new Mock<IProjectRepository>();
            _projectTaskRepository = new Mock<IProjectTaskRepository>();

            _taskCommentService = new ProjectService(_projectRepository.Object, _projectTaskRepository.Object);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(8, false)]
        public async Task Should_Get_Permission_For_Delete_Project(int tasksCount, bool allowed)
        {
            var tasksFromProject = _fixture.CreateMany<ProjectTask>(tasksCount);

            _projectTaskRepository.Setup(x => x.GetAllFromProjectAsync(It.IsAny<Guid>(), It.IsAny<Status[]>()))
                .ReturnsAsync(tasksFromProject);

            var result = await _taskCommentService.AllowDeleteAsync(Guid.NewGuid());

            result.Should().Be(allowed);

            _projectTaskRepository.Verify(x => x.GetAllFromProjectAsync(It.IsAny<Guid>(), It.IsAny<Status[]>()), Times.Once);
        }

        [Theory]
        [InlineData(6, true)]
        [InlineData(19, true)]
        [InlineData(20, false)]
        [InlineData(21, false)]
        public async Task Should_Get_Permission_For_Add_New_Task_To_Project(int tasksCount, bool allowed)
        {
            var tasksFromProject = _fixture.CreateMany<ProjectTask>(tasksCount);

            _projectTaskRepository.Setup(x => x.GetAllFromProjectAsync(It.IsAny<Guid>(), It.IsAny<Status[]>()))
                .ReturnsAsync(tasksFromProject);

            var result = await _taskCommentService.AllowNewTaskAsync(Guid.NewGuid());

            result.Should().Be(allowed);

            _projectTaskRepository.Verify(x => x.GetAllFromProjectAsync(It.IsAny<Guid>(), It.IsAny<Status[]>()), Times.Once);
        }

        [Fact]
        public async Task Should_Create_Project()
        {
            var projectDto = _fixture.Create<ProjectDto>();
            var expected = projectDto.ToEntity();

            _projectRepository.Setup(x => x.CreateAsync(It.IsAny<Project>()))
                .ReturnsAsync(expected);

            var result = await _taskCommentService.CreateAsync(projectDto);

            result.Should().BeEquivalentTo(expected);
            _projectRepository.Verify(x => x.CreateAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task Should_Verify_Project_Not_Found_When_Delete()
        {
            await _taskCommentService.DeleteAsync(Guid.NewGuid());

            _projectRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectRepository.Verify(x => x.DeleteAsync(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task Should_Delete_Project()
        {
            var project = _fixture.Create<Project>();
            var expected = project.ToDto();

            _projectRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(project);

            await _taskCommentService.DeleteAsync(project.Id);

            _projectRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectRepository.Verify(x => x.DeleteAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task Should_Get_Project()
        {
            var project = _fixture.Create<Project>();
            var expected = project.ToDto();

            _projectRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(project);

            var result = await _taskCommentService.GetAsync(project.Id);

            result.Should().BeEquivalentTo(expected);
            _projectRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
