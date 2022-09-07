using System.ComponentModel.DataAnnotations;

namespace TodoApp.RestFullAPI.Models
{
    public class TodoItem
    {
        public int TodoItemId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int AppUserId { get; set; }
    }
}
