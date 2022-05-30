using AutenticacionService.Api.Utils;
using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/profiles")]
    [Produces("application/json")]
    public class ProfilesController : ControllerBase
    {
        private readonly IUserClientServiceCommand _userClientServiceCommand;
        private readonly IUserAtelierServiceCommand _userAtelierServiceCommand;

        public ProfilesController(IUserClientServiceCommand userClientServiceCommand, IUserAtelierServiceCommand userAtelierServiceCommand)
        {
            _userClientServiceCommand = userClientServiceCommand;
            _userAtelierServiceCommand = userAtelierServiceCommand;
        }

        [HttpPost("update-client-profile")]
        [ProducesResponseType(typeof(UserClientRead), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<UserClientRead>> UpdateProfileClient(UserClientUpdate userClientUpdate)
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

                var result = await _userClientServiceCommand.Update(userClientUpdate);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.UPDATE_USER_FAILED,
                        message = ErrorMessages.UPDATE_USER_FAILED
                    });
                }

                return Ok(result);
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

        [HttpPost("upload-client-image/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<string>> UploadImageProfile(int id, [FromForm] IFormFile imageFile)
        {
            try
            {
                var result = await _userClientServiceCommand.UploadImageProfile(id, imageFile);

                if (result == null)
                {
                    return BadRequest(new ErrorDetail
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        errorCode = ErrorsCode.ADD_IMAGE_PROFILE,
                        message = ErrorMessages.ADD_IMAGE_PROFILE
                    });
                }

                return Ok(result);
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

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorDevDetail), 400)]
        [ProducesResponseType(typeof(ErrorDevDetail), 500)]
        public async Task<ActionResult<bool>> ResetPassword([FromForm] string userEmail)
        {
            try
            {
                var result = await _userAtelierServiceCommand.ResetPassword(userEmail);
                return Ok(result);
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
