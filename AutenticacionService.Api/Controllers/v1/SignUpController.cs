using AutenticacionService.Api.Utils;
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
        private readonly TokenBuilder _tokenBuilder;

        public SignUpController(IUserClientServiceCommand userClientServiceCommand, TokenBuilder tokenBuilder)
        {
            _userClientServiceCommand = userClientServiceCommand;
            _tokenBuilder = tokenBuilder;
        }

        [HttpPost("users-client")]
        [ProducesResponseType(typeof(UserClientToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserClientToken>> SignUpUserClient(UserClientCreate userClientCreate)
        {
            if (ModelState.IsValid)
            {
                var result = await _userClientServiceCommand.Create(userClientCreate);
                var userToken = _tokenBuilder.BuildClientToken(result);
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
    }
}
