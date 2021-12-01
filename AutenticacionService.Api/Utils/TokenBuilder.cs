using AutenticacionService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Utils
{
    public class TokenBuilder
    {
        private readonly IConfiguration _configuration;

        public TokenBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserClientToken BuildClientToken(UserClientRead userClientRead)
        {
            var token = BuildToken(userClientRead.UserId, userClientRead.Email, userClientRead.Role);

            return new UserClientToken
            {
                Id = userClientRead.Id,
                Email = userClientRead.Email,
                NameUser = userClientRead.NameUser,
                LastNameUser = userClientRead.LastNameUser,
                Height = userClientRead.Height,
                Phone = userClientRead.Phone,
                Role = userClientRead.Role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
            };
        }

        private JwtSecurityToken BuildToken(string id, string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return token;
        }
    }
}
