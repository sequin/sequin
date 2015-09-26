namespace Sequin
{
    using System;

    internal class ExceptionResponse
    {
        public ExceptionResponse(Exception exception)
        {
            Message = exception.Message;
        }
        
        public string Message { get; }
    }
}
