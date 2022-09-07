using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.DTOs
{
    public class PutTodoItemDTO
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
    }
}
