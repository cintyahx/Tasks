using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;
using Miotto.Tasks.Service;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service
{
    public class ProjectTaskServiceTests
    {
        private readonly Fixture _fixture;

        private readonly ProjectTaskService _projectTaskService;

        private readonly Mock<IProjectTaskRepository> _projectTaskRepository;

        public ProjectTaskServiceTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            _projectTaskRepository = new Mock<IProjectTaskRepository>();

            _projectTaskService = new ProjectTaskService(_projectTaskRepository.Object);
        }

        [Fact]
        public async Task Should_Create_Task()
        {
            var taskDto = _fixture.Create<ProjectTaskDto>();
            var expected = taskDto.ToEntity();

            _projectTaskRepository.Setup(x => x.CreateAsync(It.IsAny<ProjectTask>()))
                .ReturnsAsync(expected);

            var result = await _projectTaskService.CreateAsync(taskDto);

            result.Should().BeEquivalentTo(expected);
            _projectTaskRepository.Verify(x => x.CreateAsync(It.IsAny<ProjectTask>()), Times.Once);
        }

        [Fact]
        public async Task Should_Delete_Task()
        {
            var task = _fixture.Create<ProjectTask>();

            _projectTaskRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(task);

            await _projectTaskService.DeleteAsync(task.Id);

            _projectTaskRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectTaskRepository.Verify(x => x.DeleteAsync(It.IsAny<ProjectTask>()), Times.Once);
        }

        [Fact]
        public async Task Should_Verify_Task_Not_Found_When_Delete()
        {
            await _projectTaskService.DeleteAsync(Guid.NewGuid());

            _projectTaskRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectTaskRepository.Verify(x => x.DeleteAsync(It.IsAny<ProjectTask>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(7)]
        public async Task Should_Get_All_Tasks_From_Project(int tasksCount)
        {
            var tasksFromProject = _fixture.CreateMany<ProjectTask>(tasksCount);
            var expected = tasksFromProject.Select(task => task.ToDto());

            _projectTaskRepository.Setup(x => x.GetAllFromProjectAsync(It.IsAny<Guid>()))
                .ReturnsAsync(tasksFromProject);

            var result = await _projectTaskService.GetAllFromProjectAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expected);
            _projectTaskRepository.Verify(x => x.GetAllFromProjectAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Should_Get_Task()
        {
            var task = _fixture.Create<ProjectTask>();
            var expected = task.ToDto();

            _projectTaskRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(task);

            var result = await _projectTaskService.GetAsync(task.Id);

            result.Should().BeEquivalentTo(expected);
            _projectTaskRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Should_Update_Task()
        {
            var task = _fixture.Create<ProjectTaskDto>();
            var expected = task.ToEntity();

            _projectTaskRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expected);

            await _projectTaskService.UpdateAsync(task);

            _projectTaskRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectTaskRepository.Verify(x => x.UpdateAsync(It.IsAny<ProjectTask>()), Times.Once);
        }

        [Fact]
        public async Task Should_Verify_Task_Not_Found_When_Update()
        {
            await _projectTaskService.UpdateAsync(new ProjectTaskDto());

            _projectTaskRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _projectTaskRepository.Verify(x => x.UpdateAsync(It.IsAny<ProjectTask>()), Times.Never);
        }
    }
}
