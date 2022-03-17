using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AutenticacionService.Persistence.Handlers
{
    public class RepositoryException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public int ErrorCode { get; private set; }
        public string RepositoryMessage { get; private set; }
        public string ContentType { get; private set; }

        public RepositoryException(HttpStatusCode statusCode, int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
            ContentType = @"application/json";
        }

        public RepositoryException(HttpStatusCode statusCode, int errorCode, string message, Exception inner) : this(statusCode, errorCode, inner.Message)
        {
            RepositoryMessage = message;
            ContentType = @"application/json";
        }
    }
}
