namespace Sequin.Owin
{
    using System;

    public class ResponseMiddleware
    {
        public ResponseMiddleware(Type middlewareType, params object[] arguments)
        {
            MiddlewareType = middlewareType;
            Arguments = arguments;
        }

        public Type MiddlewareType { get; }
        public object[] Arguments { get; }
    }
}
