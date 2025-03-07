using UserPostsAPI.Models;

public static class ModelExtensions
{
    public static UserModel Clone(this UserModel original)
    {
        return new UserModel
        {
            Id = original.Id,
            Name = original.Name,
            Email = original.Email,
            Password = original.Password,
            Address = original.Address
        };
    }

    public static UserPostModel Clone(this UserPostModel original)
    {
        return new UserPostModel
        {
            Id = original.Id,
            UserId = original.UserId,
            PostContent = original.PostContent
        };
    }
}
