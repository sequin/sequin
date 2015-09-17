namespace Sequin.Infrastructure
{
    using System;

    public class CommandPipelineStage
    {
        public CommandPipelineStage(Type middlewareType, params object[] arguments)
        {
            MiddlewareType = middlewareType;
            Arguments = arguments;
        }

        public Type MiddlewareType { get; }

        public object[] Arguments { get; }
    }
}