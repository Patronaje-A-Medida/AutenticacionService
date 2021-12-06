using AutenticacionService.Api.Utils;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/sign-up")]
    [Produces("application/json")]
    public class SignUpController : ControllerBase
    {
        private readonly IUserClientServiceCommand _userClientServiceCommand;
        private readonly IUserAtelierServiceCommand _userAtelierServiceCommand;
        private readonly TokenBuilder _tokenBuilder;

        public SignUpController(
            IUserClientServiceCommand userClientServiceCommand, 
            TokenBuilder tokenBuilder,
            IUserAtelierServiceCommand userAtelierServiceCommand)
        {
            _userClientServiceCommand = userClientServiceCommand;
            _tokenBuilder = tokenBuilder;
            _userAtelierServiceCommand = userAtelierServiceCommand;
        }

        [HttpPost("users-client")]
        [ProducesResponseType(typeof(UserClientToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserClientToken>> SignUpUserClient([FromBody] UserClientCreate userClientCreate)
        {
            if (!ModelState.IsValid)
            {
                string err = string.Join(
                    "; ",
                    ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));

                return BadRequest(new { StatusCode = 400, ErrorCode = 10001, ErroMessage = err });
            }

            var result = await _userClientServiceCommand.Create(userClientCreate);
            var userToken = _tokenBuilder.BuildClientToken(result);
            return Ok(userToken);
        }

        [HttpPost("users-owner")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserAtelierToken>> SignUpUserOwner([FromBody] UserOwnerCreate userOwnerCreate)
        {
            if (!ModelState.IsValid)
            {
                string err = string.Join(
                    "; ",
                    ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));

                return BadRequest(new { StatusCode = 400, ErrorCode = 10001, ErroMessage = err });
            }

            var result = await _userAtelierServiceCommand.CreateOwner(userOwnerCreate);
            var userToken = _tokenBuilder.BuildAtelierToken(result);
            return Ok(userToken);
        }

        [HttpPost("users-technician")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserAtelierToken>> SignUpUserTechnician([FromBody] UserTechnicianCreate userTechnicianCreate)
        {
            if (!ModelState.IsValid)
            {
                string err = string.Join(
                    "; ",
                    ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));

                return BadRequest(new { StatusCode = 400, ErrorCode = 10001, ErroMessage = err });
            }

            var result = await _userAtelierServiceCommand.CreateTechnician(userTechnicianCreate);
            var userToken = _tokenBuilder.BuildAtelierToken(result);
            return Ok(userToken);
        }

        [HttpGet("1-test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CLIENT")]
        public IActionResult Test()
        {
            return Ok("prueba jwt scheme - CLIENT");
        }

        [HttpGet("2-test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "OWNER")]
        public IActionResult Test2()
        {
            return Ok("prueba jwt scheme - OWNER");
        }

        [HttpGet("3-test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TECHNICIAN")]
        public IActionResult Test3()
        {
            return Ok("prueba jwt scheme - TECHNICIAN");
        }
    }
}
