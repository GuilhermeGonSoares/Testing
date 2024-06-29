namespace TODO.Entities.Repositories;

public interface IUserRepository
{   
    Task<User?> GetByIdAsync(int id);
    void Add(User user);
    void Remove(User user);
}