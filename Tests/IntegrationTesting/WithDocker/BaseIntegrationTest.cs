using Microsoft.Extensions.DependencyInjection;
using TODO.Database;

namespace Tests.IntegrationTesting.WithDocker;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ApplicationDbContext DbContext;
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
            
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}