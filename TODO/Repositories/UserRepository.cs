using Microsoft.EntityFrameworkCore;
using TODO.Database;
using TODO.Entities;
using TODO.Entities.Repositories;

namespace TODO.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.Include(u => u.AllTodos).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.AsNoTracking().ToListAsync();
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public void Remove(User user)
    {
        context.Users.Remove(user);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}