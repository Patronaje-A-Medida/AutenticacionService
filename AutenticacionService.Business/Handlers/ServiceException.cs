using AutenticacionService.Persistence.Handlers;
using System;
using System.Net;

namespace AutenticacionService.Business.Handlers
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public int ErrorCode { get; private set; }
        public string ServiceMessage { get; private set; }
        public string ContentType { get; private set; }

        public ServiceException(HttpStatusCode statusCode, int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
            ServiceMessage = message;
            ContentType = @"application/json";
        }

        public ServiceException(HttpStatusCode statusCode, int errorCode, string message, Exception inner) : this(statusCode, errorCode, inner.Message)
        {
            ServiceMessage = message;
            ContentType = @"application/json";
        }

        public ServiceException(HttpStatusCode statusCode, RepositoryException inner) : this(statusCode, inner.ErrorCode, inner.RepositoryMessage, inner)
        {
            ContentType = @"application/json";
        }

        /*public ServiceException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }*/
    }
}
