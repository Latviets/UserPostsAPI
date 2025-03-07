public class SeedDataTests
{
    [Fact]
    public void Initialize_PopulatesDatabase_WithExpectedData()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);

        Assert.Equal(4, context.Users.Count());
        Assert.Equal(7, context.Posts.Count());
    }

    [Fact]
    public void Initialize_DoesNotDuplicateData_WhenRunMultipleTimes()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);
        SeedData.Initialize(serviceProvider.Object);

        Assert.Equal(4, context.Users.Count()); // Count from seed data
        Assert.Equal(7, context.Posts.Count());
    }

    [Fact]
    public void Initialize_AssociatesPostsWithCorrectUsers()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);

        var edvinsPosts = context.Posts
            .Where(p => p.UserId == context.Users.Single(u => u.Name == "Edvins").Id)
            .ToList();

        Assert.Equal(2, edvinsPosts.Count); // Post count for Edvins
        Assert.Contains(edvinsPosts, p => p.PostContent == "My first post");
    }

    [Fact]
    public async Task Initialize_DoesNotCreateDuplicates_WhenCalledConcurrently()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++) // Simulate 5 concurrent calls
        {
            tasks.Add(Task.Run(() => SeedData.Initialize(serviceProvider.Object)));
        }
        await Task.WhenAll(tasks); // Wait for all tasks to complete

        Assert.Equal(4, context.Users.Count());
        Assert.Equal(7, context.Posts.Count());
        Assert.True(context.Posts.All(p => p.UserId != 0));
    }
}
