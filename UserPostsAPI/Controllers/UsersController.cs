using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPostsAPI.DBContext;
using UserPostsAPI.DTO;
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


    // Additional requests if necesary
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserModel>> CreateUser(CreateUserDto userDto)
    {
        var user = new UserModel
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password,
            Address = userDto.Address
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // POST: api/users/{id}/posts
    [HttpPost("{id}/posts")]
    public async Task<IActionResult> CreateUserPosts(int id, List<CreatePostDto> postDtos)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var posts = postDtos.Select(dto => new UserPostModel
        {
            UserId = id,
            PostContent = dto.PostContent
        }).ToList();

        _context.Posts.AddRange(posts);
        await _context.SaveChangesAsync();

        return Ok(posts);
    }
}
