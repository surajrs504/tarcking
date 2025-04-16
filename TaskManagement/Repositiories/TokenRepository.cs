using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManagement.Repositiories
{
    public class TokenRepository : ITokenInterface
    {
        private readonly IConfiguration configuration;
        public TokenRepository(IConfiguration IConfiguration)
        {
            configuration = IConfiguration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //we add the claims for the tokens - these are the user info that are found when we decode a 
            //jwt token
            var claims =new List<Claim>();
            claims.Add(new Claim("email", user.Email));
            claims.Add(new Claim("username", user.UserName));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             configuration["Jwt:Issuer"],
             configuration["Jwt:Audience"],
             claims,
             expires: DateTime.Now.AddMinutes(15),
             signingCredentials: credential
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
