using TODO.Entities;
using TODO.Exceptions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Tests.UnitTesting.Entities;

public class UserSpec
{
    [Fact]
    public void ShouldCreateUser()
    {
        //arrange
        var username = "GuiGo";
        //act
        var user = new User(username);
        //assert
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
        Assert.Empty(user.AllTodos);
    }

    [Fact]
    public void ShouldNotCreateUserWithEmptyOrNullUsername()
    {
        //arrange
        var username = "";
        //act
        var exception = Assert.Throws<ValidationException>(() => new User(username));
        //assert
        Assert.Equal("Username is required", exception.Message);

        //arrange
        string username2 = null!;
        //act & assert
        Assert.Throws<ValidationException>(() => new User(username2));
    }

    [Fact]
    public void ShouldBeAbleToAddTodo()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        var todo2 = new Todo("Title2", "Description2") { Id = 2 };
        var user = new User("GuiGo");
        //act
        user.AddTodo(todo);
        //assert
        Assert.Single(user.AllTodos);
        //act
        user.AddTodo(todo2);
        //assert
        Assert.Equal(2, user.AllTodos.Count);
    }

    [Fact]
    public void ShouldThrowExceptionWhenTodoAlreadyExists()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        var user = new User("GuiGo");
        user.AddTodo(todo);
        //act & assert
        Assert.Throws<AlreadyExist>(() => user.AddTodo(todo));
    }

    [Fact]
    public void ShouldBeAbleToRemoveTodo()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        var todo2 = new Todo("Title2", "Description2") { Id = 2 };
        var user = new User("GuiGo");
        user.AddTodo(todo);
        user.AddTodo(todo2);
        //act
        user.RemoveTodoById(1);
        //assert
        Assert.Single(user.AllTodos);
        //act
        user.RemoveTodoById(2);
        //assert
        Assert.Empty(user.AllTodos);
        //act
        user.RemoveTodoById(3);
        //assert
        Assert.Empty(user.AllTodos);
    }

    [Fact]
    public void ShouldUpdateTodo()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        var user = new User("GuiGo");
        user.AddTodo(todo);
        const string newTitle = "New Title";
        const string newDescription = "New Description";
        //act
        user.UpdateTodoById(1, newTitle, newDescription);
        //assert
        Assert.Equal(newTitle, user.AllTodos.First(t => t.Id == 1).Title);
        Assert.Equal(newDescription, user.AllTodos.First(t => t.Id == 1).Description);
    }
    
    [Fact]
    public void ShouldNotUpdateTodoWhenTodoNotFound()
    {
        //arrange
        var user = new User("GuiGo");
        //act & assert
        Assert.Throws<NotFoundException>(() => user.UpdateTodoById(1, "Title", "Description"));
    }
    
    [Fact]
    public void ShouldMarkTodoAsCompleted()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        var user = new User("GuiGo");
        user.AddTodo(todo);
        //act
        user.MarkTodoAsCompleted(1);
        //assert
        Assert.True(user.AllTodos.First(t => t.Id == 1).IsCompleted);
    }
    
    [Fact]
    public void ShouldNotMarkTodoAsCompletedWhenTodoNotFound()
    {
        //arrange
        var user = new User("GuiGo");
        //act & assert
        Assert.Throws<NotFoundException>(() => user.MarkTodoAsCompleted(1));
    }
    
    [Fact]
    public void ShouldMarkTodoAsMarkTodoAsUncompleted()
    {
        //arrange
        var todo = new Todo("Title", "Description") { Id = 1 };
        todo.MarkAsCompleted();
        var user = new User("GuiGo");
        user.AddTodo(todo);
        //act
        user.MarkTodoAsUncompleted(1);
        //assert
        Assert.False(user.AllTodos.First(t => t.Id == 1).IsCompleted);
    }
    
    [Fact]
    public void ShouldNotMarkTodoAsUncompletedWhenTodoNotFound()
    {
        //arrange
        var user = new User("GuiGo");
        //act & assert
        Assert.Throws<NotFoundException>(() => user.MarkTodoAsUncompleted(1));
    }
}