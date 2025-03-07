using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPostsAPI.DBContext;
using UserPostsAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUserById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


    // GET: api/users/{id}/posts
    [HttpGet("{id}/posts")]
    public async Task<ActionResult<IEnumerable<UserPostModel>>> GetUserPosts(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var userPosts = await _context.Posts.Where(p => p.UserId == id).ToListAsync();

        if (!userPosts.Any())
        {
            return NotFound();
        }

        return userPosts;
    }
}
