using FluentAssertions;

public class SeedDataTests
{
    [Fact]
    public void Initialize_PopulatesDatabase_WithExpectedData()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);

        context.Users.Count().Should().Be(4);
        context.Posts.Count().Should().Be(7);
    }

    [Fact]
    public void Initialize_DoesNotDuplicateData_WhenRunMultipleTimes()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);
        SeedData.Initialize(serviceProvider.Object);

        context.Users.Count().Should().Be(4); // Count from seed data
        context.Posts.Count().Should().Be(7);
    }

    [Fact]
    public void Initialize_AssociatesPostsWithCorrectUsers()
    {
        var (context, serviceProvider) = TestUtils.SetupInMemoryDatabase();

        SeedData.Initialize(serviceProvider.Object);

        var edvinsPosts = context.Posts
            .Where(p => p.UserId == context.Users.Single(u => u.Name == "Edvins").Id)
            .ToList();

        edvinsPosts.Count.Should().Be(2); // Post count for Edvins
        edvinsPosts.Should().Contain(p => p.PostContent == "My first post");
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

        context.Users.Count().Should().Be(4);
        context.Posts.Count().Should().Be(7);
        context.Posts.All(p => p.UserId != 0).Should().BeTrue();
    }
}
