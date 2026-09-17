using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    // GET api/todos
    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetAll()
    {
        return Ok(_todoService.GetAll());
    }

    // GET api/todos/5
    [HttpGet("{id:int}")]
    public ActionResult<TodoItem> GetById(int id)
    {
        var todo = _todoService.GetById(id);
        return todo is null ? NotFound() : Ok(todo);
    }

    // POST api/todos
    [HttpPost]
    public ActionResult<TodoItem> Create([FromBody] CreateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Title is required.");
        }

        var todo = _todoService.Create(dto.Title);
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    // PUT api/todos/5
    [HttpPut("{id:int}")]
    public ActionResult<TodoItem> Update(int id, [FromBody] UpdateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Title is required.");
        }

        var todo = _todoService.Update(id, dto.Title, dto.IsComplete);
        return todo is null ? NotFound() : Ok(todo);
    }

    // PATCH api/todos/5/toggle
    [HttpPatch("{id:int}/toggle")]
    public ActionResult<TodoItem> Toggle(int id)
    {
        var existing = _todoService.GetById(id);
        if (existing is null) return NotFound();

        var todo = _todoService.SetComplete(id, !existing.IsComplete);
        return Ok(todo);
    }

    // DELETE api/todos/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _todoService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
