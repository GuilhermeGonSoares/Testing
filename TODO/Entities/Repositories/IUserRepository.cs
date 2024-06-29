namespace TODO.Entities.Repositories;

public interface IUserRepository
{   
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    void Add(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}