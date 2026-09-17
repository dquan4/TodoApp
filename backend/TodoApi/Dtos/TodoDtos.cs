using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public class CreateTodoDto
{
    [Required, MinLength(1), MaxLength(200)]
    public string Title { get; set; } = string.Empty;
}

public class UpdateTodoDto
{
    [Required, MinLength(1), MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; }
}
