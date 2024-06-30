using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TODO.Database;
using TODO.Entities;
using TODO.Repositories;
using TODO.Services;

namespace Tests.IntegrationTesting.WithoutDocker;

public class UserServiceInMemorySpec
    : IClassFixture<IntegrationTestInMemoryWebAppFactory>
{
    private readonly UserService _userService;
    private readonly ApplicationDbContext _dbContext;
    
    public UserServiceInMemorySpec(IntegrationTestInMemoryWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _userService = new UserService(new UserRepository(_dbContext));
    }
    [Fact]
    public async Task ShouldCreateUser()
    {
        //arrange
        var user = new User("CreateUserTest") { Id = 1 };
        //act
        await _userService.AddUserAsync(user.Username);
        //assert
        var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == 1);
        Assert.NotNull(createdUser);
        Assert.Equal(user.Username, createdUser.Username);
    }
    
    [Fact]
    public async Task ShouldGetAllUsers()
    {   
        //arrange
        var user = new User("TestUser-1");
        var user2 = new User("TestUser-2");
        await _dbContext.Users.AddRangeAsync(user, user2);
        await _dbContext.SaveChangesAsync();
        //act
        var users = await _userService.GetUsersAsync();
        //assert
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Contains(user.Username, users.Select(u => u.Username));  
        Assert.Contains(user2.Username, users.Select(u => u.Username));
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        //arrange
        var newUser = new User("GuiGo") { Id = 10 };
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        //act
        var user = await _userService.GetUserByIdAsync(10);
        //assert
        Assert.NotNull(user);
        Assert.Equal(10, user.Id);
        Assert.Equal("GuiGo", user.Username);
    }
    
    [Fact]
    public async Task ShouldRemoveUser()
    {
        //arrange
        var newUser = new User("GuiGo") { Id = 11 };
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        //act
        await _userService.RemoveUserAsync(11);
        //assert
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == 11);
        Assert.Null(user);
    }

    [Fact]
    public async Task ShouldBeAbleToAddTodoToUser()
    {
        //arrange
        var user = new User("GuiGo") { Id = 12 };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var title = "AddTodoTitle";
        var description = "AddTodoDescription";
        //act
        await _userService.AddTodoAsync(12, title, description);
        var userWithTodo = await _userService.GetUserByIdAsync(12);
        //assert
        Assert.NotNull(userWithTodo);
        Assert.NotEmpty(userWithTodo.AllTodos);
        Assert.Contains(title, userWithTodo.AllTodos.Select(t => t.Title));
        Assert.Contains(description, userWithTodo.AllTodos.Select(t => t.Description));
    }

    [Fact]
    public async Task ShouldBeAbleToRemoveTodoFromUser()
    {
        //arrange
        var user = new User("GuiGo") { Id = 13 };
        var todo = new Todo("RemoveTodoTitle", "RemoveTodoDescription"){ Id = 2 };
        var todo2 = new Todo("RemoveTodoTitle2", "RemoveTodoDescription2"){ Id = 3 };
        user.AddTodo(todo);
        user.AddTodo(todo2);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        //act
        await _userService.RemoveTodoAsync(user.Id, todo.Id);
        var userWithoutTodo = await _userService.GetUserByIdAsync(user.Id);
        //assert
        Assert.NotNull(userWithoutTodo);
        Assert.Single(userWithoutTodo.AllTodos);
        Assert.DoesNotContain(todo.Title, userWithoutTodo.AllTodos.Select(t => t.Title));
    }
    
    [Fact]
    public async Task ShouldBeAbleToUpdateTodoFromUser()
    {
        //arrange
        var user = new User("GuiGo") { Id = 14 };
        var todo = new Todo("UpdateTodoTitle", "UpdateTodoDescription"){ Id = 4 };
        user.AddTodo(todo);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var newTitle = "NewTitle";
        var newDescription = "NewDescription";
        //act
        await _userService.UpdateTodoAsync(user.Id, todo.Id, newTitle, newDescription);
        var userWithUpdatedTodo = await _userService.GetUserByIdAsync(user.Id);
        //assert
        Assert.NotNull(userWithUpdatedTodo);
        Assert.Single(userWithUpdatedTodo.AllTodos);
        Assert.Contains(newTitle, userWithUpdatedTodo.AllTodos.Select(t => t.Title));
        Assert.Contains(newDescription, userWithUpdatedTodo.AllTodos.Select(t => t.Description));
    }
    
    [Fact]
    public async Task ShouldBeAbleToMarkTodoAsCompleted()
    {
        //arrange
        var user = new User("GuiGo") { Id = 15 };
        var todo = new Todo("MarkTodoTitle", "MarkTodoDescription"){ Id = 5 };
        user.AddTodo(todo);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        //act
        await _userService.MarkTodoAsCompletedAsync(user.Id, todo.Id);
        var userWithMarkedTodo = await _userService.GetUserByIdAsync(user.Id);
        //assert
        Assert.NotNull(userWithMarkedTodo);
        Assert.Single(userWithMarkedTodo.AllTodos);
        Assert.True(userWithMarkedTodo.AllTodos.First().IsCompleted);
    }

    [Fact]
    public async Task ShouldBeAbleToMarkTodoAsUncompleted()
    {
        //arrange
        var user = new User("GuiGo") { Id = 16 };
        var todo = new Todo("MarkTodoTitle", "MarkTodoDescription") { Id = 6 };
        todo.MarkAsCompleted();
        user.AddTodo(todo);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        //act
        await _userService.MarkTodoAsUnCompleteAsync(user.Id, todo.Id);
        var userWithMarkedTodo = await _userService.GetUserByIdAsync(user.Id);
        //assert
        Assert.NotNull(userWithMarkedTodo);
        Assert.Single(userWithMarkedTodo.AllTodos);
        Assert.False(userWithMarkedTodo.AllTodos.First().IsCompleted);
    }
}