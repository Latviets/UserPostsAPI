using UserPostsAPI.Data.Models;

public static class ModelExtensions
{
    public static User Clone(this User original)
    {
        return new User
        {
            Id = original.Id,
            Name = original.Name,
            Email = original.Email,
            Password = original.Password,
            Address = original.Address
        };
    }

    public static UserPost Clone(this UserPost original)
    {
        return new UserPost
        {
            Id = original.Id,
            UserId = original.UserId,
            PostContent = original.PostContent
        };
    }
}
