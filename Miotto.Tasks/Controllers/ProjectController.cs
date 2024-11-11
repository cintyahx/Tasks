using Microsoft.AspNetCore.Mvc;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;

namespace Miotto.Tasks.API.Controllers
{
    [ApiController]
    [Route("api/v1/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}", Name = nameof(ProjectController) + nameof(Details))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var project = await _projectService.GetAsync(id);

            return project == null
                    ? NotFound()
                    : Ok(project);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProjectDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProjectDto createProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = await _projectService.CreateAsync(createProjectDto);

            return CreatedAtRoute(nameof(ProjectController) + nameof(Details), new { id = project.Id }, project);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
    }
}
