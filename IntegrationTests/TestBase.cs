using Microsoft.EntityFrameworkCore;
using UserPostsAPI.DBContext;

public class TestBase
{
    protected readonly AppDbContext Context;

    public TestBase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        Context = new AppDbContext(options);
    }
}
