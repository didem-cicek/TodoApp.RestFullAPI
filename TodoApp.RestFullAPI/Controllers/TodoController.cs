using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp.DTOs;
using TodoApp.RestFullAPI.Models;

namespace TodoApp.RestFullAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _context;
        public TodoController(TodoDbContext _context)
        {
            this._context = _context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;
            if(userId == null)
                return NotFound();

            var items = await _context.TodoItems.ToListAsync();
            if (!items.Any())
                return NotFound();

            var model = from q in items
                        where q.AppUserId.ToString() == userId
                        select new TodoItemDTO
                        {
                            Title = q.Title,
                            IsDone = q.IsDone,
                            TodoItemId = q.TodoItemId,
                        };
            return Ok(model);
        }
        
        [HttpGet("id")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;
            if (userId == null)
                return NotFound();

            var item = await _context.TodoItems.FindAsync(id);
            
            if (item == null)
                return NotFound();
            
            var findUser = await _context.TodoItems.FirstOrDefaultAsync(a=>a.AppUserId == Convert.ToInt32(userId));
            
            if (findUser == null)
                return NotFound();
            var model = new TodoItemDTO
            {
                Title = item.Title,
                IsDone = item.IsDone,
                TodoItemId = item.TodoItemId,
            };
            return Ok(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> PostTodoItemAsync(PostTodoItemDTO dto)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;
            if (userId == null)
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest();
            var item = new TodoItem
            {
                Title = dto.Title,
                IsDone = dto.IsDone,
                AppUserId = Convert.ToInt32(userId),
            };
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            var returnValue = CreatedAtAction("GetTodoItem",
                new { id = item.TodoItemId },
                new TodoItemDTO
                {
                    Title = item.Title,
                    IsDone = item.IsDone,
                    TodoItemId = item.TodoItemId
                });
            return returnValue;
        }
        
        [HttpPut("{id}")]
        
        public async Task<ActionResult> PutTodoItemAsync(int id, PutTodoItemDTO dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;
            if (userId == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            if(id!=dto.TodoItemId)
                return BadRequest();
            var item = await _context.TodoItems.FirstOrDefaultAsync(o=>o.TodoItemId == id && o.AppUserId == Convert.ToInt32(userId));
            if (item == null)
                return NotFound();
            item.Title = dto.Title;
            item.IsDone = dto.IsDone;
            await _context.SaveChangesAsync();
            return NoContent();

         }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItemAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;
            if (userId == null)
                return NotFound();
            var item = await _context.TodoItems.FirstOrDefaultAsync(o => o.TodoItemId == id && o.AppUserId == Convert.ToInt32(userId));
            if (item == null)
                return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
