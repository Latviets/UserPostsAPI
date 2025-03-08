using Microsoft.EntityFrameworkCore;
using Moq;
using UserPostsAPI.Data.DBContext;
using UserPostsAPI.Data.Models;

public static class DbContextMockHelper
{
    // Creates a mocked DbSet
    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockDbSet;
    }

    // Creates a mocked DbContext
    public static Mock<AppDbContext> CreateMockDbContext(
        List<User> users, List<UserPost> posts)
    {
        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Users).Returns(CreateMockDbSet(users).Object);
        mockContext.Setup(c => c.Posts).Returns(CreateMockDbSet(posts).Object);
        return mockContext;
    }

    // Creates a controller with the mocked DbContext
    public static UsersController CreateControllerWithMockDbContext(
        List<User> users, List<UserPost> posts)
    {
        var mockContext = CreateMockDbContext(users, posts);
        return new UsersController(mockContext.Object);
    }
}