using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SignInController : ControllerBase
    {
        private readonly IUserClientServiceCommand _serviceCommand;
        private readonly IConfiguration _configuration;

        public SignInController(IUserClientServiceCommand serviceCommand, IConfiguration configuration)
        {
            _serviceCommand = serviceCommand;
            _configuration = configuration;
        }

        /*[HttpPost()]
        public async Task<ActionResult<UserToken>> SignInUserClient([FromBody] UserClientCreate)
        {

        }*/

        private UserToken BuildToken(UserClientRead userClientRead)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userClientRead.Email),
                new Claim(ClaimTypes.NameIdentifier, userClientRead.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userClientRead.Role)
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
                Expiration = expiration
            };
        }
    }
}
