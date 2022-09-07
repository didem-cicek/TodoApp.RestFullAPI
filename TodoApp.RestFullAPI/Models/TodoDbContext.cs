using Microsoft.EntityFrameworkCore;

namespace TodoApp.RestFullAPI.Models
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions opt) : base(opt)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
