using TODO.Entities;
using TODO.Entities.Repositories;
using TODO.Exceptions;

namespace TODO.Services;

public class UserService(IUserRepository userRepository)
{
    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            throw new NotFoundException("User not found");
        return user;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await userRepository.GetAllAsync();
    }
    
    public async Task AddUserAsync(string username)
    {
        var user = new User(username);
        userRepository.Add(user);
        await userRepository.SaveChangesAsync();
    }
    
    public async Task RemoveUserAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is not null)
        {
            userRepository.Remove(user);
            await userRepository.SaveChangesAsync();
        }
    }
    
    public async Task AddTodoAsync(int userId, string title, string description)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("User not found");
        user.AddTodo(new Todo(title, description));
        await userRepository.SaveChangesAsync();
    }
    
    public async Task RemoveTodoAsync(int userId, int todoId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.RemoveTodoById(todoId);
        await userRepository.SaveChangesAsync();
    }
    
    public async Task UpdateTodoAsync(int userId, int todoId, string title, string description)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.UpdateTodoById(todoId, title, description);
        await userRepository.SaveChangesAsync();
    }
    
    public async Task MarkTodoAsCompletedAsync(int userId, int todoId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.MarkTodoAsCompleted(todoId);
        await userRepository.SaveChangesAsync();
    }
    
    public async Task MarkTodoAsUnCompleteAsync(int userId, int todoId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.MarkTodoAsUncompleted(todoId);
        await userRepository.SaveChangesAsync();
    }
}