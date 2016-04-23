namespace Sequin.Owin
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Owin;
    using Pipeline;

    internal class ExecuteCommandPipeline : OwinMiddleware
    {
        private readonly CommandPipeline pipeline;

        public ExecuteCommandPipeline(OwinMiddleware next, CommandPipeline pipeline) : base(next)
        {
            this.pipeline = pipeline;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();
            await DynamicExecutePipeline(command);
        }

        private Task DynamicExecutePipeline(object command)
        {
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => ExecutePipeline(x);
                var methodCallExpression = (MethodCallExpression)expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                var task = (Task)methodInfo.Invoke(this, new[] { command });
                return task;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return Task.FromResult(0);
        }

        private Task ExecutePipeline<TCommand>(TCommand command)
        {
            return pipeline.Execute(command);
        }
    }
}
