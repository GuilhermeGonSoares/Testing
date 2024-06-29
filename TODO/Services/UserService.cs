using TODO.Entities;
using TODO.Entities.Repositories;
using TODO.Exceptions;

namespace TODO.Services;

public class UserService(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            throw new NotFoundException("User not found");
        return user;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    
    public async Task AddUserAsync(string username)
    {
        var user = new User(username);
        _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task RemoveUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is not null)
        {
            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
        }
    }
    
    public async Task AddTodoAsync(int userId, string title, string description)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("User not found");
        user.AddTodo(new Todo(title, description));
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task RemoveTodoAsync(int userId, int todoId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.RemoveTodoById(todoId);
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task UpdateTodoAsync(int userId, int todoId, string title, string description)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.UpdateTodoById(todoId, title, description);
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task MarkTodoAsCompletedAsync(int userId, int todoId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.MarkTodoAsCompleted(todoId);
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task MarkTodoAsUnCompleteAsync(int userId, int todoId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        user.MarkTodoAsUncompleted(todoId);
        await _userRepository.SaveChangesAsync();
    }
}