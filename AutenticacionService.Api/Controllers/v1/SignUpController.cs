using AutenticacionService.Api.Utils;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [ProducesResponseType(typeof(ErrorDetail), 500)]
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
        [ProducesResponseType(typeof(ErrorDetail), 500)]
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
        [ProducesResponseType(typeof(ErrorDetail), 500)]
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

        /*[HttpGet("1-test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CLIENT")]
        public IActionResult Test()
        {
            return Ok("prueba jwt scheme - CLIENT");
        }*/

        [HttpPost("test-sign-up")]
        public ActionResult<UserAtelierToken> TestPorquesi([FromBody] UserOwnerCreate userOwnerCreate)
        {
            var ss = new UserAtelierToken
            {
                UserInfo = new UserAtelierRead
                {
                    Id = 1,
                    UserId = "abcdexys",
                    NameUser = userOwnerCreate.NameUser,
                    LastNameUser = userOwnerCreate.LastNameUser,
                    Dni = userOwnerCreate.Dni,
                    AtelierId = 1,
                    Email = userOwnerCreate.Email,
                    Role = "OWNER"
                },

                UserToken = new UserToken
                {
                   Expiration = DateTime.Now,
                   Token = "SUPERRECOTNRARCHMEGRAULTRASECRETOTROKEN"
                }
            };

            return Ok(ss);
        }

        [HttpGet("error-test")]
        public IActionResult TestPorqueNo()
        {
            var err = new ErrorDetail
            {
                statusCode = 500,
                errorCode = 10001,
                message = "error forzado"
            };

            return BadRequest(err);
        }

    }
}
