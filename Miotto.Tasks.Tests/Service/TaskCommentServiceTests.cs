using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Domain.Mappings;
using Miotto.Tasks.Service;
using Moq;
using System.Globalization;

namespace Miotto.Tasks.Tests.Service
{
    public class TaskCommentServiceTests
    {
        private readonly Fixture _fixture;

        private readonly ITaskCommentService _taskCommentService;

        private readonly Mock<ITaskCommentRepository> _taskCommentRepository;

        public TaskCommentServiceTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();

            _taskCommentRepository = new Mock<ITaskCommentRepository>();

            _taskCommentService = new TaskCommentService(_taskCommentRepository.Object);
        }

        [Fact]
        public async Task Should_Create_Task()
        {
            var commentDto = _fixture.Create<TaskCommentDto>();
            var expected = commentDto.ToEntity();

            _taskCommentRepository.Setup(x => x.CreateAsync(It.IsAny<TaskComment>()))
                .ReturnsAsync(expected);

            var result = await _taskCommentService.CreateAsync(commentDto);

            result.Should().BeEquivalentTo(expected);
            _taskCommentRepository.Verify(x => x.CreateAsync(It.IsAny<TaskComment>()), Times.Once);
        }

        [Fact]
        public async Task Should_Delete_Comment()
        {
            var comment = _fixture.Create<TaskComment>();

            _taskCommentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(comment);

            await _taskCommentService.DeleteAsync(comment.Id);

            _taskCommentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _taskCommentRepository.Verify(x => x.DeleteAsync(It.IsAny<TaskComment>()), Times.Once);
        }

        [Fact]
        public async Task Should_Verify_Comment_Not_Found_When_Delete()
        {
            await _taskCommentService.DeleteAsync(Guid.NewGuid());

            _taskCommentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _taskCommentRepository.Verify(x => x.DeleteAsync(It.IsAny<TaskComment>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task Should_Get_All_Comments_From_Task(int commentsCount)
        {
            var commentsFromTask = _fixture.CreateMany<TaskComment>(commentsCount);
            var expected = commentsFromTask.Select(comment => comment.ToDto());

            _taskCommentRepository.Setup(x => x.GetAllFromTaskAsync(It.IsAny<Guid>()))
                .ReturnsAsync(commentsFromTask);

            var result = await _taskCommentService.GetAllFromTaskAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expected);
            _taskCommentRepository.Verify(x => x.GetAllFromTaskAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Should_Get_Task()
        {
            var comment = _fixture.Create<TaskComment>();
            var expected = comment.ToDto();

            _taskCommentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(comment);

            var result = await _taskCommentService.GetAsync(comment.Id);

            result.Should().BeEquivalentTo(expected);
            _taskCommentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Should_Update_Comment()
        {
            var comment = _fixture.Create<TaskCommentDto>();
            var expected = comment.ToEntity();

            _taskCommentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expected);

            await _taskCommentService.UpdateAsync(comment);

            _taskCommentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _taskCommentRepository.Verify(x => x.UpdateAsync(It.IsAny<TaskComment>()), Times.Once);
        }

        [Fact]
        public async Task Should_Verify_Comment_Not_Found_When_Update()
        {
            await _taskCommentService.UpdateAsync(new TaskCommentDto());

            _taskCommentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
            _taskCommentRepository.Verify(x => x.UpdateAsync(It.IsAny<TaskComment>()), Times.Never);
        }
    }
}
