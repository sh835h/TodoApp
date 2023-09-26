using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static AuthServiceRepository;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private static readonly List<User> _inMemoryDatabase = new List<User>();

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthenticationModel model)
    {
        try
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_inMemoryDatabase.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email already registered.");
            }

            
            var newUser = new User
            {
                Email = model.Email,
                Password = model.Password 
            };

            
            _inMemoryDatabase.Add(newUser);
            Console.WriteLine($"{_inMemoryDatabase}");

          
            var token = GenerateJwtToken(newUser.Email, new string[] { "User" });

            return Ok(new { Token = token, Message = "Registration successful." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User model) 
    {
        try
        {
          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            Console.WriteLine($"Login attempt with email: {model.Email}");

         
            var user = _inMemoryDatabase.FirstOrDefault(u => u.Email == model.Email);

         
            Console.WriteLine($"Found user: {user?.Email}");


            if (user != null && user.Password == model.Password)
            {
                
                user.Password = null;

                var token = GenerateJwtToken(user.Email, new string[] { "User" });

                return Ok(new { Token = token, Message = "Login successful." });
            }
            else
            {
                return Unauthorized("Invalid email or password.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    private string GenerateJwtToken(string email, string[] roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
