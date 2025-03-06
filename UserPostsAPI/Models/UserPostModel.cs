namespace UserPostsAPI.Models
{
    public class UserPostModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? PostContent { get; set; }
    }
}
