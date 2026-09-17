using TodoApi.Models;

namespace TodoApi.Services;

/// <summary>
/// Simple thread-safe in-memory store. Data lives only for the lifetime of the
/// process; no persistence is required for this assessment.
/// </summary>
public class InMemoryTodoService : ITodoService
{
    private readonly List<TodoItem> _todos = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public InMemoryTodoService()
    {
        // Seed a couple of items so the app isn't empty on first load.
        _todos.Add(new TodoItem { Id = _nextId++, Title = "Review pull requests", IsComplete = false });
        _todos.Add(new TodoItem { Id = _nextId++, Title = "Set up local dev environment", IsComplete = true });
    }

    public IEnumerable<TodoItem> GetAll()
    {
        lock (_lock)
        {
            return _todos
                .OrderBy(t => t.IsComplete)
                .ThenByDescending(t => t.CreatedAt)
                .ToList();
        }
    }

    public TodoItem? GetById(int id)
    {
        lock (_lock)
        {
            return _todos.FirstOrDefault(t => t.Id == id);
        }
    }

    public TodoItem Create(string title)
    {
        lock (_lock)
        {
            var todo = new TodoItem
            {
                Id = _nextId++,
                Title = title.Trim(),
                IsComplete = false,
                CreatedAt = DateTime.UtcNow
            };
            _todos.Add(todo);
            return todo;
        }
    }

    public TodoItem? Update(int id, string title, bool isComplete)
    {
        lock (_lock)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return null;

            todo.Title = title.Trim();
            todo.IsComplete = isComplete;
            return todo;
        }
    }

    public TodoItem? SetComplete(int id, bool isComplete)
    {
        lock (_lock)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return null;

            todo.IsComplete = isComplete;
            return todo;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return false;

            return _todos.Remove(todo);
        }
    }
}
