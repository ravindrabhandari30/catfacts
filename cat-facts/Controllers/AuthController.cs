using cat_facts.Model;
using cat_facts.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// Handles authentication-related operations such as login and token generation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Configuration object to access appsettings (e.g., JWT settings)
    private readonly IConfiguration _config;

    // Service responsible for generating JWT tokens
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="config">Application configuration</param>
    /// <param name="tokenService">JWT token service</param>
    public AuthController(IConfiguration config, ITokenService tokenService)
    {
        _config = config;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Authenticates the user and returns a JWT token if credentials are valid.
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    /// <returns>JWT token if successful, otherwise Unauthorized</returns>
    [HttpPost("login")]
    public IActionResult Login(string username, string password)
    {
        // ⚠️ NOTE:
        // In a real-world application:
        // - Validate user credentials from a database
        // - Use hashed passwords (e.g., BCrypt or ASP.NET Identity)
        // - Never store plain text passwords

        // Demo validation (hardcoded credentials)
        if (username == "admin" && password == "password")
        {
            // Generate JWT token using token service
            var token = _tokenService.GenerateToken(username);

            // Return token in response
            return Ok(new { Token = token });
        }

        // Return Unauthorized if credentials are invalid
        return Unauthorized("Invalid credentials");
    }
}