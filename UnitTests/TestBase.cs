using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using UserPostsAPI.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using UserPostsAPI.Data.Models;

public abstract class TestBase
{
    protected readonly ServiceProvider ServiceProvider;

    public TestBase()
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddTransient<IValidator<User>, UserValidator>();
        services.AddTransient<IValidator<UserPost>, UserPostValidator>();


        ServiceProvider = services.BuildServiceProvider();
    }

    protected T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }
}
