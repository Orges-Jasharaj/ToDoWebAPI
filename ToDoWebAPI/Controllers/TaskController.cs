using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoWebAPI.Dtos.Responses;
using ToDoWebAPI.Service.Interface;

namespace ToDoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var tasks = await _taskService.GetTasksByUserIdAsync(currentUserId);
            return Ok(tasks ?? new List<TaskDto>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto newTask)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User ID not found in token.");

            newTask.CreatedBy = currentUserId;
            if (string.IsNullOrWhiteSpace(newTask.UserId))
            {
                newTask.UserId = currentUserId;
            }

            var created = await _taskService.CreateTaskAsync(newTask);
            return Ok(created);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] CreateTaskDto updatedTask)
        {
            updatedTask.UserId = string.IsNullOrWhiteSpace(updatedTask.UserId)
                ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                : updatedTask.UserId;
            return Ok(await _taskService.UpdateTaskAsync(id, updatedTask));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/complete")]

        public async Task<IActionResult> MarkTaskAsCompleted(string id, [FromBody] string completedBy)
        {
            return Ok(await _taskService.MarkTaskAsCompletedAsync(id, completedBy));
        }


    }
}
