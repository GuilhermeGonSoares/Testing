using System.ComponentModel.DataAnnotations;
using TODO.Entities;

namespace Tests.UnitTesting.Entities;

public class TodoSpec
{
    [Fact]
    public void ShouldCreateTodo()
    {
        //arrange
        const string title = "Unit Testing";
        const string description = "Write unit tests for Todo entity";
        //act
        var todo = new Todo(title, description);
        //assert
        Assert.NotNull(todo);
        Assert.Equal(title, todo.Title);
        Assert.Equal(description, todo.Description);
        Assert.False(todo.IsCompleted);
        Assert.IsType<DateTime>(todo.CreatedAt);
        Assert.IsType<DateTime>(todo.UpdatedAt);
    }
    
    [Fact]
    public void ShouldThrowExceptionWhenTitleOrDescriptionIsNullOrEmpty()
    {
        //arrange
        const string title = "";
        const string description = "Write unit tests for Todo entity";
        //act & assert
        Assert.Throws<ValidationException>(() => new Todo(title, description));
        
        //arrange
        const string title2 = "Unit Testing";
        const string description2 = "";
        //act & assert
        Assert.Throws<ValidationException>(() => new Todo(title2, description2));
    }

    [Fact]
    public void ShouldBeAbleToMarkAsCompleted()
    {
        //arrange
        var todo = new Todo("Unit Testing", "Write unit tests for Todo entity");
        //act
        todo.MarkAsCompleted();
        //assert
        Assert.True(todo.IsCompleted);
    }

    [Fact]
    public void ShouldBeAbleToMarkAsUncompleted()
    {
        //arrange
        var todo = new Todo("Study tests ", "I need to study tests");
        //act
        todo.MarkAsUncompleted();
        //assert
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public void ShouldUpdateTodo()
    {
        //arrange
        var todo = new Todo("Play video-game", "I want to play LOL");
        var updatedAt = todo.UpdatedAt;
        var newTitle = "Play video-game";
        var newDescription = "I want to play Valorant";
        //act
        todo.Update(newTitle, newDescription);
        //assert
        Assert.Equal(newTitle, todo.Title);
        Assert.Equal(newDescription, todo.Description);
        Assert.NotEqual(updatedAt, todo.UpdatedAt);
    }
}