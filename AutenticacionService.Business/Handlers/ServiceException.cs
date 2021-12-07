using AutenticacionService.Persistence.Handlers;
using System;
using System.Net;

namespace AutenticacionService.Business.Handlers
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public int ErrorCode { get; private set; }
        public string ContentType { get; private set; }

        public ServiceException(HttpStatusCode statusCode, int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
            ContentType = @"application/json";
        }

        public ServiceException(HttpStatusCode statusCode, int errorCode, Exception inner) : this(statusCode, errorCode, inner.ToString())
        {
            ContentType = @"application/json";
        }

        public ServiceException(HttpStatusCode statusCode, RepositoryException inner) : this(statusCode, inner.ErrorCode, inner.Message)
        {
            ContentType = @"application/json";
        }

        /*public ServiceException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }*/
    }
}
