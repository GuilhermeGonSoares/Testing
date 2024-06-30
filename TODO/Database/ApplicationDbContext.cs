using Microsoft.EntityFrameworkCore;
using TODO.Entities;

namespace TODO.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Username).IsRequired();
            entity.HasMany(u => u.AllTodos).WithOne().HasForeignKey("UserId").IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();
            entity.Property(t => t.Title).IsRequired();
            entity.Property(t => t.Description).IsRequired();
            entity.Property(t => t.IsCompleted).HasDefaultValue(false);
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt).IsRequired();
        });
    }
}