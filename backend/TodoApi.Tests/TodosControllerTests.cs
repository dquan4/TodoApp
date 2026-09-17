using Microsoft.AspNetCore.Mvc;
using TodoApi.Controllers;
using TodoApi.Dtos;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests;

public class TodosControllerTests
{
    [Fact]
    public void GetById_returns_not_found_when_todo_does_not_exist()
    {
        var controller = new TodosController(new InMemoryTodoService());

        var result = controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_rejects_blank_title_without_calling_service()
    {
        var service = new RecordingTodoService();
        var controller = new TodosController(service);

        var result = controller.Create(new CreateTodoDto { Title = "   " });

        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Null(service.CreatedTitle);
    }

    [Fact]
    public void Create_returns_created_result_for_valid_title()
    {
        var controller = new TodosController(new InMemoryTodoService());

        var result = controller.Create(new CreateTodoDto { Title = "New task" });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(TodosController.GetById), created.ActionName);
        Assert.Equal(3, created.RouteValues!["id"]);
    }

    [Fact]
    public void Toggle_returns_not_found_for_unknown_todo()
    {
        var controller = new TodosController(new InMemoryTodoService());

        var result = controller.Toggle(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Delete_returns_no_content_when_delete_succeeds()
    {
        var controller = new TodosController(new InMemoryTodoService());

        var result = controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    private sealed class RecordingTodoService : ITodoService
    {
        public string? CreatedTitle { get; private set; }

        public IEnumerable<TodoApi.Models.TodoItem> GetAll() => [];
        public TodoApi.Models.TodoItem? GetById(int id) => null;

        public TodoApi.Models.TodoItem Create(string title)
        {
            CreatedTitle = title;
            return new TodoApi.Models.TodoItem { Id = 1, Title = title };
        }

        public TodoApi.Models.TodoItem? Update(int id, string title, bool isComplete) => null;
        public TodoApi.Models.TodoItem? SetComplete(int id, bool isComplete) => null;
        public bool Delete(int id) => false;
    }
}
