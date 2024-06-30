using TODO.Exceptions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace TODO.Entities;

public class User : IAggregateRoot
{
    private readonly List<Todo> _todos = [];
    public int Id { get; init; }
    public string Username { get; private set;}
    public ICollection<Todo> AllTodos => _todos.ToList();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public User()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public User(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ValidationException("Username is required");
        Username = username;
        _todos = [];
    }
    public void AddTodo(Todo newTodo)
    {
        var existedTodo = _todos.FirstOrDefault(t => t.Id == newTodo.Id);
        if (existedTodo is not null)
            throw new AlreadyExist("Todo already exists");  
        _todos.Add(newTodo);
    }
    
    public void RemoveTodoById(int todoId)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoId);
        if (todo is null)
            return;
        _todos.Remove(todo);
    }
    
    public void UpdateTodoById(int todoId, string title, string description)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoId);
        if (todo is null)
            throw new NotFoundException("Todo not found");
        todo.Update(title, description);
    }
    
    public void MarkTodoAsCompleted(int todoId)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoId);
        if (todo is null)
            throw new NotFoundException("Todo not found");
        todo.MarkAsCompleted();
    }
    
    public void MarkTodoAsUncompleted(int todoId)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoId);
        if (todo is null)
            throw new NotFoundException("Todo not found");
        todo.MarkAsUncompleted();
    }
}