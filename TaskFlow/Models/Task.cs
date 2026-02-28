using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [StringLength(1000)]
        public string Description { get; set; } = "";

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        public TaskStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? Deadline { get; set; }
        public DateTime? CompletedAt { get; set; }

        [Required]
        public string Creator { get; set; } = "";

        public string? Assignee { get; set; }
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum TaskStatus
    {
        New = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}
