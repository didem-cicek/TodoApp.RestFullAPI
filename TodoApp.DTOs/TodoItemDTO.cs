namespace TodoApp.DTOs
{
    public class TodoItemDTO
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int AppUserId { get; set; }
    }
}