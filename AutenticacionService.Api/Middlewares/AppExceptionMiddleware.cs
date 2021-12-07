using AutenticacionService.Api.Utils;
using AutenticacionService.Business.Handlers;
using AutenticacionService.Persistence.Handlers;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Api.Middlewares
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AppExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(RepositoryException repoEx)
            {
                await HandleExceptionAsync(context, repoEx);
            }
            catch (ServiceException servEx)
            {
                await HandleExceptionAsync(context, servEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, RepositoryException ex)
        {
            string result;
            context.Response.ContentType = "application/json";

            if(ex is RepositoryException)
            {
                result = new ErrorDetail
                {
                    StatusCode = (int)ex.StatusCode,
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                }.ToString();

                context.Response.StatusCode = (int)ex.StatusCode;
            }
            else
            {
                result = new ErrorDetail
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorCode = ErrorsCode.GENERIC_ERROR,
                    Message = ErrorMessages.GENERIC_ERROR,
                }.ToString();

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, ServiceException ex)
        {
            string result;
            context.Response.ContentType = "application/json";

            if (ex is ServiceException)
            {
                result = new ErrorDetail
                {
                    StatusCode = (int)ex.StatusCode,
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                }.ToString();

                context.Response.StatusCode = (int)ex.StatusCode;
            }
            else
            {
                result = new ErrorDetail
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorCode = ErrorsCode.GENERIC_ERROR,
                    Message = ErrorMessages.GENERIC_ERROR,
                }.ToString();

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result = new ErrorDetail
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorCode = ErrorsCode.GENERIC_ERROR,
                Message = ex.Message
            }.ToString();

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
