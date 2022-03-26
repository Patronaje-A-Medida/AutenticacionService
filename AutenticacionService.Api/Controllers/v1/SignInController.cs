using AutenticacionService.Api.Utils;
using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesQuerys.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

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

        [HttpGet("prueba-docker")]
        public IActionResult PruebaDocker()
        {
            return Ok("prueba docker");
        }

        [HttpPost("users-client")]
        [ProducesResponseType(typeof(UserClientToken), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<UserClientToken>> SignInUserClient([FromBody] UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string err = string.Join(
                        "; ",
                        ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));

                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.INVALID_MODEL_ERROR,
                        message = err
                    });
                }

                var result = await _userClientServiceQuery.SignIn(userLogin);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.LOGIN_USER_INVALID,
                        message = ErrorMessages.LOGIN_USER_INVALID
                    });
                }

                var userToken = _tokenBuilder.BuildClientToken(result);
                return Ok(userToken);
            }
            catch (ServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("users-atelier")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<UserAtelierToken>> SignInUserAtelier([FromBody] UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string err = string.Join(
                        "; ",
                        ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage));

                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.INVALID_MODEL_ERROR,
                        message = err
                    });
                }

                var result = await _userAtelierServiceQuery.SignIn(userLogin);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.LOGIN_USER_INVALID,
                        message = ErrorMessages.LOGIN_USER_INVALID
                    });
                }

                var userToken = _tokenBuilder.BuildAtelierToken(result);
                return Ok(userToken);
            }
            catch (ServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
