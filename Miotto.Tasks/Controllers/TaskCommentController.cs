using Microsoft.AspNetCore.Mvc;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;

namespace Miotto.Tasks.API.Controllers
{
    [ApiController]
    [Route("api/v1/task/{taskId}/comment")]
    public class TaskCommentController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentService;

        public TaskCommentController(ITaskCommentService taskCommentService)
        {
            _taskCommentService = taskCommentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskCommentDto))]
        public async Task<IActionResult> GetAll(Guid taskId)
        {
            return Ok(await _taskCommentService.GetAllFromTaskAsync(taskId));
        }

        [HttpGet("{id}", Name = nameof(TaskCommentController) + nameof(Details))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectTaskDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskCommentService.GetAsync(id);

            return task == null
                    ? NotFound()
                    : Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskCommentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Guid taskId, TaskCommentDto createTaskCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            createTaskCommentDto.TaskId = taskId;

            var comment = await _taskCommentService.CreateAsync(createTaskCommentDto);

            return CreatedAtRoute(nameof(ProjectTaskController) + nameof(Details), new { taskId, id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Edit(Guid taskId, Guid id, TaskCommentDto taskCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            taskCommentDto.Id = id;
            taskCommentDto.TaskId = taskId;

            var comment = await _taskCommentService.UpdateAsync(taskCommentDto);

            return comment == null
                    ? NotFound()
                    : NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskCommentService.DeleteAsync(id);

            return NoContent();
        }
    }
}
