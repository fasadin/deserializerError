using System;
using System.Collections.Generic;

namespace deserializer
{
    public abstract class Error
    {
        #region BaseError 
        public abstract string ErrorType { get; set; }
        public abstract int StatusCode { get; set; }
        public IList<string> Messages { get; set; } = new List<string>();
        public abstract string? StackTrace { get; set; }
        
        private Error(string errorType, int statusCode)
        {
            ErrorType = errorType;
            StatusCode = statusCode;
            string stacktr = Environment.StackTrace.Substring(700, 1000);
            string[] lines = stacktr.Split(" at ");
            StackTrace = lines[1];
        }

        private Error(string errorType, int statusCode, IList<string> messages) : this(errorType, statusCode)
        {
            Messages = messages;
        }

        private Error(string errorType, int statusCode, string message) : this(errorType, statusCode)
        {
            Messages = new List<string>() { message };
        }

        public void AppendMessage(string message)
        {
            Messages.Add(message);
        }
        #endregion
        
        
        #region BadRequest
        
        public sealed class BadRequestError : Error
        {
            public BadRequestError(IList<string> messages, string? stackTrace) 
                : base(nameof(BadRequestError), 400, messages)
            {
                StackTrace = stackTrace;
                ErrorType = nameof(BadRequestError);
                StatusCode = 400;
            }

            public BadRequestError(string message, string? stackTrace) 
                : base(nameof(BadRequestError), 400, message)
            {
                StackTrace = stackTrace;
                ErrorType = nameof(BadRequestError);
                StatusCode = 400;
            }


            public override string ErrorType { get; set; }
            public override int StatusCode { get; set; }
            public override string? StackTrace { get; set; }
        }
        
        public static Error BadRequest(IList<string> messages, string? stackTrace = null)
            => new BadRequestError(messages, stackTrace);
        
        public static Error BadRequest(string message, string? stackTrace = null)
            => new BadRequestError(message, stackTrace);

        #endregion
        
        #region ServiceError
        
        public sealed class ServiceGeneralError : Error
        {
            public ServiceGeneralError(IList<string> messages, string? stackTrace = null) 
                : base(nameof(ServiceGeneralError), 503, messages)
            {
                StackTrace = stackTrace;
                ErrorType = nameof(ServiceGeneralError);
                StatusCode = 500;
            }

            public ServiceGeneralError(string message, string? stackTrace = null) 
                : base(nameof(ServiceUnavailableError), 500, message)
            {
                StackTrace = stackTrace;
                StatusCode = 500;
                ErrorType = nameof(ServiceUnavailableError);
            }

            public override string ErrorType { get; set; }
            public override int StatusCode { get; set; }
            public override string? StackTrace { get; set; }
        }
        
        public static Error ServiceError(IList<string> messages, string? stackTrace = null)
            => new ServiceUnavailableError(messages, stackTrace);
        
        public static Error ServiceError(string message, string? stackTrace = null)
            => new ServiceUnavailableError(message, stackTrace);

        #endregion

        
        #region ServiceUnavailable
        
        public sealed class ServiceUnavailableError : Error
        {
            public ServiceUnavailableError(IList<string> messages, string? stackTrace) 
                : base(nameof(ServiceUnavailableError), 503, messages)
            {
                StackTrace = stackTrace;
                ErrorType = nameof(ServiceUnavailableError);
                StatusCode = 503;
            }

            public ServiceUnavailableError(string message, string? stackTrace) 
                : base(nameof(ServiceUnavailableError), 503, message)
            {
                StackTrace = stackTrace;
                StatusCode = 503;
                ErrorType = nameof(ServiceUnavailableError);
            }

            public override string ErrorType { get; set; }
            public override int StatusCode { get; set; }
            public override string? StackTrace { get; set; }
        }
        
        public static Error ServiceUnavailable(IList<string> messages, string? stackTrace = null)
            => new ServiceUnavailableError(messages, stackTrace);
        
        public static Error ServiceUnavailable(string message, string? stackTrace = null)
            => new ServiceUnavailableError(message, stackTrace);

        #endregion
        
        
        #region NotFound
        
        public sealed class NotFoundError : Error
        {
            public NotFoundError(IList<string> messages, string? stackTrace = null)
                : base(nameof(NotFoundError), 404, messages)
            {
                StackTrace = stackTrace;
                StatusCode = 404;
                ErrorType = nameof(NotFoundError);
            }

            public NotFoundError(string message, string? stackTrace) 
                : base(nameof(NotFoundError), 404, message)
            {
                StackTrace = stackTrace;
                StatusCode = 404;
                ErrorType = nameof(NotFoundError);
            }

            public override string ErrorType { get; set; }
            public override int StatusCode { get; set; }
            public override string? StackTrace { get; set; }
        }
        
        public static Error NotFound(IList<string> messages, string? stackTrace = null)
            => new NotFoundError(messages, stackTrace);
        
        public static Error NotFound(string message, string? stackTrace = null)
            => new NotFoundError(message, stackTrace);

        #endregion
        
        
        #region Unauthorized
        
        public sealed class UnauthorizedError : Error
        {
            public UnauthorizedError(IList<string> messages, string? stackTrace) 
                : base(nameof(UnauthorizedError), 401, messages)
            {
                StackTrace = stackTrace;
                StatusCode = 401;
                ErrorType = nameof(NotFoundError);
            }

            public UnauthorizedError(string message, string? stackTrace)
                : base(nameof(UnauthorizedError), 401, message)
            {
                StackTrace = stackTrace;
                StatusCode = 401;
                ErrorType = nameof(NotFoundError);
            }

            public override string ErrorType { get; set; }
            public override int StatusCode { get; set; }
            public override string? StackTrace { get; set; }
        }
        
        public static Error Unauthorized(IList<string> messages, string? stackTrace = null)
            => new UnauthorizedError(messages, stackTrace);
        
        public static Error Unauthorized(string message, string? stackTrace = null)
            => new UnauthorizedError(message, stackTrace);

        #endregion
    }
}