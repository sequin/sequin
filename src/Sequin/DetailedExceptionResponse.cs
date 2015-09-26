namespace Sequin
{
    using System;

    internal class DetailedExceptionResponse : ExceptionResponse
    {
        public DetailedExceptionResponse(Exception exception) : base(exception)
        {
            Type = exception.GetType().FullName;
            StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
            {
                InnerException = new ExceptionResponse(exception.InnerException);
            }
        }

        public string Type { get; }
        public string StackTrace { get; }
        public ExceptionResponse InnerException { get; }
    }
}
