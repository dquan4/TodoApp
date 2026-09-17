using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests;

public class InMemoryTodoServiceTests
{
    [Fact]
    public void Constructor_seeds_incomplete_items_before_completed_items()
    {
        var service = new InMemoryTodoService();

        var todos = service.GetAll().ToList();

        Assert.Equal(2, todos.Count);
        Assert.False(todos[0].IsComplete);
        Assert.True(todos[1].IsComplete);
    }

    [Fact]
    public void Create_trims_title_assigns_next_id_and_starts_incomplete()
    {
        var service = new InMemoryTodoService();

        var todo = service.Create("  Buy groceries  ");

        Assert.Equal(3, todo.Id);
        Assert.Equal("Buy groceries", todo.Title);
        Assert.False(todo.IsComplete);
        Assert.Same(todo, service.GetById(todo.Id));
    }

    [Fact]
    public void Update_changes_title_and_completion_state()
    {
        var service = new InMemoryTodoService();

        var updated = service.Update(1, "  Updated task ", true);

        Assert.NotNull(updated);
        Assert.Equal("Updated task", updated.Title);
        Assert.True(updated.IsComplete);
    }

    [Fact]
    public void SetComplete_returns_null_for_unknown_id()
    {
        var service = new InMemoryTodoService();

        var updated = service.SetComplete(999, true);

        Assert.Null(updated);
    }

    [Fact]
    public void Delete_removes_existing_item_and_reports_unknown_id()
    {
        var service = new InMemoryTodoService();

        Assert.True(service.Delete(1));
        Assert.Null(service.GetById(1));
        Assert.False(service.Delete(999));
    }
}
