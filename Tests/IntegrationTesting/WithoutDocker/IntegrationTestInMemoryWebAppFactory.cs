using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TODO.Database;

namespace Tests.IntegrationTesting.WithoutDocker;

public class IntegrationTestInMemoryWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Encontre o serviço de DbContext e remova-o
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            // Adicione o DbContext com Postgresql test
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=todo-test;Username=test-postgres;Password=test-postgres");
            });
        });
    }

    public async Task InitializeAsync()
    {
        // Migrate the database
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        // Drop the database
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }
}