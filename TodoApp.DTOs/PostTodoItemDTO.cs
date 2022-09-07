using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.DTOs
{
    public class PostTodoItemDTO
    {
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int AppUserId { get; set; }
    }
}
