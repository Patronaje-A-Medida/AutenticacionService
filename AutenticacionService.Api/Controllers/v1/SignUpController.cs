using AutenticacionService.Api.Utils;
using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

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
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<UserClientToken>> SignUpUserClient([FromBody] UserClientCreate userClientCreate)
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

                var result = await _userClientServiceCommand.Create(userClientCreate);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.SIGN_UP_INVALID,
                        message = ErrorMessages.SIGN_UP_INVALID
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

        [HttpPost("users-owner")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<UserAtelierToken>> SignUpUserOwner([FromBody] UserOwnerCreate userOwnerCreate)
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

                var result = await _userAtelierServiceCommand.CreateOwner(userOwnerCreate);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.SIGN_UP_INVALID,
                        message = ErrorMessages.SIGN_UP_INVALID
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

        [HttpPost("users-technician")]
        [ProducesResponseType(typeof(UserAtelierToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(ErrorDetail), 500)]
        public async Task<ActionResult<UserAtelierToken>> SignUpUserTechnician([FromBody] UserTechnicianCreate userTechnicianCreate)
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

                var result = await _userAtelierServiceCommand.CreateTechnician(userTechnicianCreate);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.SIGN_UP_ERROR,
                        message = ErrorMessages.SIGN_UP_ERROR
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
