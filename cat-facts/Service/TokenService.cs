using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cat_facts.Service
{
    /// <summary>
    /// Service responsible for generating JSON Web Tokens (JWT)
    /// used for authenticating users in the application.
    /// </summary>
    public class TokenService : ITokenService
    {
        // Provides access to configuration settings (e.g., JWT settings in appsettings.json)
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor with dependency injection for configuration
        /// </summary>
        /// <param name="config">Application configuration</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">Username or user identifier</param>
        /// <returns>Signed JWT token string</returns>
        public string GenerateToken(string user)
        {
            // Retrieve JWT configuration values (Key, Issuer, Audience, Subject)
            var jwtSettings = _config.GetSection("Jwt");

            // Create a symmetric security key using the secret key from configuration
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            
            Console.WriteLine("Issuer: " + jwtSettings["Issuer"]);
            Console.WriteLine("Audience: " + jwtSettings["Audience"]);
            Console.WriteLine("Key: " + jwtSettings["Key"]);

            
            // Define the signing credentials using HMAC SHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims (information stored inside the token)
            var claims = new[]
            {
                // Subject (typically identifies the purpose of the token)
                new Claim(JwtRegisteredClaimNames.Sub, jwtSettings["Subject"]),

                // Custom claims (you can add more like roles, email, etc.)
                new Claim("id", user),
                new Claim("username", user)
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],          // Token issuer
                audience: jwtSettings["Audience"],      // Token audience
                claims: claims,                         // Claims to include
                expires: DateTime.UtcNow.AddMinutes(60),// Token expiration time
                signingCredentials: creds               // Signing credentials
            );

            // Convert the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}