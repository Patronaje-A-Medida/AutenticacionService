using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/v1/sign-up")]
    [Produces("application/json")]
    public class SignUpController : ControllerBase
    {
        private readonly IUserClientServiceCommand _userClientServiceCommand;
        private readonly IConfiguration _configuration;

        public SignUpController(IUserClientServiceCommand userClientServiceCommand, IConfiguration configuration)
        {
            _userClientServiceCommand = userClientServiceCommand;
            _configuration = configuration;
        }

        [HttpPost("users-client")]
        [ProducesResponseType(typeof(UserClientToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserToken>> SignUpUserClient(UserClientCreate userClientCreate)
        {
            if (ModelState.IsValid)
            {
                var result = await _userClientServiceCommand.Create(userClientCreate);
                var userToken = BuildToken(result);
                return Ok(userToken);
            }
            else
            {
                string err = string.Join(
                    "; ",
                    ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));

                return BadRequest(new { StatusCode = 400, ErrorCode = 10001, ErroMessage = err });
            }
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CLIENTE")]
        public IActionResult Test()
        {
            return Ok("prueba jwt scheme");
        }

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
