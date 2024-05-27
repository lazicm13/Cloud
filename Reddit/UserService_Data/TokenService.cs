using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserService_Data
{
    public class TokenService
    {
        private readonly string _securityKey;

        public TokenService(string securityKey)
        {
            _securityKey = securityKey;
        }

        public TokenService()
        {

        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("long_secret_key_with_at_least_32_bytes"));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Ime),
            new Claim(ClaimTypes.Email, user.Email),
            
           
            };

            var token = new JwtSecurityToken(
                issuer: "your_app",
                audience: "your_client",
                claims: claims,
                expires: DateTime.Now.AddHours(1), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool ValidateToken(string token)
        {
            if (token == null)
            {
                // Handle case where token is null
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securityKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Možete postaviti toleranciju za vreme
                }, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

                return true; // Token je validan
            }
            catch (Exception)
            {
                return false; // Token nije validan ili je istekao
            }
        }

        public string GetUsernameFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (usernameClaim != null && emailClaim != null)
                {
                    string username = usernameClaim.Value;
                    return username;
                }
                else
                {
                    Console.WriteLine("Error: Required claims not found in token.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Greška prilikom dekodiranja tokena
                Console.WriteLine("Error decoding token: " + ex.Message + ex.StackTrace);
                return null;
            }
        }

        public string GetEmailFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (usernameClaim != null && emailClaim != null)
                {
                    string email = emailClaim.Value;
                    return email;
                }
                else
                {
                    Console.WriteLine("Error: Required claims not found in token.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Greška prilikom dekodiranja tokena
                Console.WriteLine("Error decoding token: " + ex.Message + ex.StackTrace);
                return null;
            }
        }

    }
}
