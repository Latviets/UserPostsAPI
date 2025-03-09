namespace UserPostsAPI.Data.Models
{
    public class UserPost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? PostContent { get; set; }
    }
}