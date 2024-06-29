using Moq;
using TODO.Entities;
using TODO.Entities.Repositories;
using TODO.Exceptions;
using TODO.Services;

namespace Tests.UnitTesting.Services;

public class UserServiceSpec
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserService _sut;
    
    public UserServiceSpec()
    {
        _sut = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnAllUser()
    {   
        //arrange
        var expectedUsers = new List<User>
        {
            new User("user1"),
            new User("user2")
        };
        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedUsers);
        //act
        var result = await _sut.GetUsersAsync();
        //assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ShouldReturnUserById()
    {
        //arrange
        var expectedUser = new User("user1"){Id = 1};
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(expectedUser);
        //act
        var result = await _sut.GetUserByIdAsync(1);
        //assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);
        _userRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFound()
    {
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);
        //act
        var exception = await Record.ExceptionAsync(() => _sut.GetUserByIdAsync(1));
        //assert
        Assert.NotNull(exception);
        Assert.IsType<NotFoundException>(exception);
        _userRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }
    
    [Fact]
    public async Task ShouldAddUser()
    {
        //arrange
        var username = "user1";
        //act
        await _sut.AddUserAsync(username);
        //assert
        _userRepositoryMock.Verify(x => x.Add(It.Is<User>(u => u.Username == username)), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldRemoveUser()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
        //act
        await _sut.RemoveUserAsync(1);
        //assert
        _userRepositoryMock.Verify(x => x.Remove(
            It.Is<User>(u => u.Username == user.Username)), 
            Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldAddTodoToUser()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        var title = "Study Tests";
        var description = "Improve my skills";
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);
        //act
        await _sut.AddTodoAsync(1, title, description);
        //assert
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFoundOnAddTodo()
    {
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);
        //act
        var exception = await Record.ExceptionAsync(() => _sut.AddTodoAsync(1, "Study Tests", "Improve my skills"));
        //assert
        Assert.NotNull(exception);
        Assert.IsType<NotFoundException>(exception);
    }
    
    [Fact]
    public async Task ShouldRemoveTodoFromUser()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        var todo = new Todo("Study Tests", "Improve my skills"){Id = 1};
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);
        //act
        await _sut.RemoveTodoAsync(1, 1);
        //assert
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFoundOnRemoveTodo()
    {   
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);
        //act & assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.RemoveTodoAsync(1, 1));
    }
    
    [Fact]
    public async Task ShouldUpdateTodoFromUser()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        var todo = new Todo("Test", "Testing"){Id = 1};
        user.AddTodo(todo);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);
        //act
        await _sut.UpdateTodoAsync(1, 1, "Study Tests", "Improve my skills");
        //assert
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.Equal("Study Tests", todo.Title);
        Assert.Equal("Improve my skills", todo.Description);
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFoundOnUpdateTodo()
    {
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);
        //act & assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateTodoAsync(1, 1, "Study Tests", "Improve my skills"));
    }
    
    [Fact]
    public async Task ShouldMarkTodoAsCompleted()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        var todo = new Todo("Test", "Testing"){Id = 1};
        user.AddTodo(todo);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);
        //act
        await _sut.MarkTodoAsCompletedAsync(1, 1);
        //assert
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.True(todo.IsCompleted);
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFoundOnMarkTodoAsCompleted()
    {
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);
        //act & assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.MarkTodoAsCompletedAsync(1, 1));
    }
    
    [Fact]
    public async Task ShouldMarkTodoAsUncompleted()
    {
        //arrange
        var user = new User("GuiGhosts"){Id = 1};
        var todo = new Todo("Test", "Testing"){Id = 1};
        todo.MarkAsCompleted();
        user.AddTodo(todo);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);
        //act
        await _sut.MarkTodoAsUnCompleteAsync(1, 1);
        //assert
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.False(todo.IsCompleted);
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFoundOnMarkTodoAsUncompleted()
    {
        //arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);
        //act & assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.MarkTodoAsUnCompleteAsync(1, 1));
    }
}