using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
        [Display(Name = "Название задачи")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите приоритет")]
        [Display(Name = "Приоритет")]
        public TaskPriority Priority { get; set; }

        [Required(ErrorMessage = "Выберите статус")]
        [Display(Name = "Статус")]
        public TaskStatus Status { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Дедлайн")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Дата завершения")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CompletedAt { get; set; }

        [Required(ErrorMessage = "Укажите создателя")]
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        [Display(Name = "Создатель")]
        public string Creator { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
        [Display(Name = "Исполнитель")]
        public string? Assignee { get; set; }

        [Display(Name = "Прогресс (%)")]
        [Range(0, 100, ErrorMessage = "Прогресс должен быть от 0 до 100")]
        public int Progress { get; set; }
    }

    public enum TaskPriority
    {
        [Display(Name = "Низкий")]
        Low = 1,

        [Display(Name = "Средний")]
        Medium = 2,

        [Display(Name = "Высокий")]
        High = 3
    }

    public enum TaskStatus
    {
        [Display(Name = "Новая")]
        New = 1,

        [Display(Name = "В работе")]
        InProgress = 2,

        [Display(Name = "Завершена")]
        Completed = 3,

        [Display(Name = "Отменена")]
        Cancelled = 4
    }
}
