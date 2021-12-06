using AutenticacionService.Api.Utils;
using AutenticacionService.Business.ServicesQuerys.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/sign-in")]
    [Produces("application/json")]
    public class SignInController : ControllerBase
    {
        private readonly IUserClientServiceQuery _userClientServiceQuery;
        private readonly IUserAtelierServiceQuery _userAtelierServiceQuery;
        private readonly TokenBuilder _tokenBuilder;

        public SignInController(
            IUserClientServiceQuery userClientServiceQuery, 
            IUserAtelierServiceQuery userAtelierServiceQuery, 
            TokenBuilder tokenBuilder)
        {
            _userClientServiceQuery = userClientServiceQuery;
            _userAtelierServiceQuery = userAtelierServiceQuery;
            _tokenBuilder = tokenBuilder;
        }

        [HttpPost("users-client")]
        [ProducesResponseType(typeof(UserClientToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserToken>> SignInUserClient([FromBody] UserLogin userLogin)
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

            var result = await _userClientServiceQuery.SignIn(userLogin);
            var userToken = _tokenBuilder.BuildClientToken(result);
            return Ok(userToken);
        }

        [HttpPost("users-atelier")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<UserToken>> SignInUserAtelier([FromBody] UserLogin userLogin)
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

            var result = await _userAtelierServiceQuery.SignIn(userLogin);
            var userToken = _tokenBuilder.BuildAtelierToken(result);
            return Ok(userToken);
        }

    }
}
