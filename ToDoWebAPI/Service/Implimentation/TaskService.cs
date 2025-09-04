using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Data;
using ToDoWebAPI.Data.Models;
using ToDoWebAPI.Dtos.Responses;
using ToDoWebAPI.Service.Interface;

namespace ToDoWebAPI.Service.Implimentation
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> CreateTaskAsync(CreateTaskDto newTask)
        {
            TaskItem task = new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = newTask.Title,
                Dificulty = newTask.Dificulty,
                DeadLine = newTask.DeadLine,
                Description = newTask.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = newTask.UserId,
                UserId = newTask.UserId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task created with ID: {task.Id}");
            return true;
        }

        public async Task<bool> DeleteTaskAsync(string taskId)
        {
            var existingTask = await _context.Tasks.FindAsync(taskId);
            if (existingTask == null)
            {
                return false;
            }
            _context.Tasks.Remove(existingTask);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task deleted with ID: {taskId}");
            return true;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Dificulty = t.Dificulty,
                    IsCompleted = t.IsCompleted,
                    DeadLine = t.DeadLine,
                    Description = t.Description,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    UpdatedBy = t.UpdatedBy,
                    UpdatedAt = t.UpdatedAt,
                    CompletedAt = t.CompletedAt,
                    CompletedBy = t.CompletedBy,
                    UserId = t.UserId
                })
                .ToListAsync();
        }

        public async Task<TaskDto?> GetTaskByIdAsync(string taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return null;
            }
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Dificulty = task.Dificulty,
                IsCompleted = task.IsCompleted,
                DeadLine = task.DeadLine,
                Description = task.Description,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedBy = task.UpdatedBy,
                UpdatedAt = task.UpdatedAt,
                CompletedAt = task.CompletedAt,
                CompletedBy = task.CompletedBy,
                UserId = task.UserId
            };
        }

        public async Task<List<TaskDto>> GetTasksByUserIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new List<TaskDto>();
            }
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Dificulty = t.Dificulty,
                    IsCompleted = t.IsCompleted,
                    DeadLine = t.DeadLine,
                    Description = t.Description,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    UpdatedBy = t.UpdatedBy,
                    UpdatedAt = t.UpdatedAt,
                    CompletedAt = t.CompletedAt,
                    CompletedBy = t.CompletedBy,
                    UserId = t.UserId
                })
                .ToListAsync();
        }

        public async Task<bool> MarkTaskAsCompletedAsync(string taskId, string completedBy)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsCompleted)
            {
                return false;
            }
            task.IsCompleted = true;
            task.CompletedAt = DateTime.UtcNow;
            task.CompletedBy = completedBy;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task marked as completed with ID: {taskId}");
            return true;
        }

        public async Task<bool> UpdateTaskAsync(string taskId, CreateTaskDto updatedTask)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return false;
            }
            task.Title = updatedTask.Title;
            task.Dificulty = updatedTask.Dificulty;
            task.DeadLine = updatedTask.DeadLine;
            task.Description = updatedTask.Description;
            task.UpdatedAt = DateTime.UtcNow;
            task.UpdatedBy = updatedTask.UserId;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task updated with ID: {taskId}");
            return true;    
        }
    }
}
