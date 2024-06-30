using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TODO.Entities;
using TODO.Repositories;
using TODO.Services;

namespace Tests.IntegrationTesting.WithDocker;

public class UserServiceIntegrationSpec(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]  
    public async Task ShouldCreateUser()
    {
        //arrange
        var service = new UserService(new UserRepository(
            DbContext
        ));
        var user = new User("TestUser");
        //act
        await service.AddUserAsync(user.Username);
        //assert
        var createdUser = await DbContext.Users.FirstOrDefaultAsync();
        Assert.NotNull(createdUser);
        Assert.Equal(user.Username, createdUser.Username);
    }
    
    [Fact]
    public async Task ShouldGetAllUsers()
    {
        //arrange
        var service = new UserService(new UserRepository(
            DbContext
        ));
        //act
        var users = await service.GetUsersAsync();
        //assert
        Assert.Empty(users);
    }
}