using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    IEnumerable<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem Create(string title);
    TodoItem? Update(int id, string title, bool isComplete);
    TodoItem? SetComplete(int id, bool isComplete);
    bool Delete(int id);
}
