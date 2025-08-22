using System.ComponentModel.DataAnnotations;
using ToDoWebAPI.Data.Models;

namespace ToDoWebAPI.Dtos
{
    public class TaskDto
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Dificulty { get; set; }
        public bool IsCompleted { get; set; } = false;
        [Required]
        public DateTime DeadLine { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? CompletedBy { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }

    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Dificulty { get; set; }
        [Required]
        public DateTime DeadLine { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public string UserId { get; set; }
    }
}
