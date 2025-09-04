using ToDoWebAPI.Data.Models;
using ToDoWebAPI.Dtos.Responses;

namespace ToDoWebAPI.Service.Interface
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskDto?> GetTaskByIdAsync(string taskId);
        Task<List<TaskDto>> GetTasksByUserIdAsync(string userId);
        Task<bool> CreateTaskAsync(CreateTaskDto newTask);
        Task<bool> UpdateTaskAsync(string taskId, CreateTaskDto updatedTask);
        Task<bool> DeleteTaskAsync(string taskId);
        Task<bool> MarkTaskAsCompletedAsync(string taskId, string completedBy);
    }
}
