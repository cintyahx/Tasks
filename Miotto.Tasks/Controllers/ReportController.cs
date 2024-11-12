using Microsoft.AspNetCore.Mvc;
using Miotto.Tasks.Domain.Dtos;
using Miotto.Tasks.Service;

namespace Miotto.Tasks.API.Controllers
{
    [ApiController]
    [Route("api/v1/report")]
    public class ReportController : ControllerBase
    {
        private readonly IProjectTaskService _taskProjectService;

        public ReportController(IProjectTaskService taskProjectService)
        {
            _taskProjectService = taskProjectService;
        }

        [HttpGet("tasks-done-last-month/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectTaskDto))]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            return Ok(await _taskProjectService.GetTasksDoneLastMonthAsync(userId));
        }
    }
}
