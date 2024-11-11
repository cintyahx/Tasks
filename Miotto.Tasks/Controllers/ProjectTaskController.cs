using Microsoft.AspNetCore.Mvc;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;

namespace Miotto.Tasks.API.Controllers
{
    [ApiController]
    [Route("api/v1/project/{projectId}/task")]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _taskProjectService;

        public ProjectTaskController(IProjectTaskService taskProjectService)
        {
            _taskProjectService = taskProjectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectTaskDto))]
        public async Task<IActionResult> GetAll(Guid projectId)
        {
            return Ok(await _taskProjectService.GetAllFromProjectAsync(projectId));
        }

        [HttpGet("{id}", Name = nameof(ProjectTaskController) + nameof(Details))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectTaskDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskProjectService.GetAsync(id);

            return task == null
                    ? NotFound()
                    : Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProjectTaskDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Guid projectId, ProjectTaskDto createTaskProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            createTaskProjectDto.ProjectId = projectId;

            var task = await _taskProjectService.CreateAsync(createTaskProjectDto);

            return CreatedAtRoute(nameof(ProjectTaskController) + nameof(Details), new { projectId, id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Edit(Guid projectId, Guid id, ProjectTaskDto taskProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            taskProjectDto.Id = id;
            taskProjectDto.ProjectId = projectId;

            var task = await _taskProjectService.UpdateAsync(taskProjectDto);

            return task == null
                    ? NotFound()
                    : NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskProjectService.DeleteAsync(id);

            return NoContent();
        }
    }
}
