using Microsoft.EntityFrameworkCore;
using Moq;
using UserPostsAPI.Data.DBContext;

public static class TestUtils
{
    public static (AppDbContext, Mock<IServiceProvider>) SetupInMemoryDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(sp => sp.GetService(typeof(DbContextOptions<AppDbContext>)))
            .Returns(options);

        return (context, serviceProvider);
    }

    public static (AppDbContext, UsersController) CreateTestController(string databaseName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new AppDbContext(options);
        var controller = new UsersController(context);

        return (context, controller);
    }
}