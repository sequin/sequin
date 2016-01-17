namespace Sequin.Middleware
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using CommandBus;
    using Core.Infrastructure;
    using Microsoft.Owin;
    using Extensions;

    internal class IssueCommand : OwinMiddleware
    {
        private readonly ExclusiveHandlerCommandBus commandBus;
        private readonly ICommandPostProcessor postProcessor;

        public IssueCommand(OwinMiddleware next, ExclusiveHandlerCommandBus commandBus, ICommandPostProcessor postProcessor) : base(next)
        {
            this.commandBus = commandBus;
            this.postProcessor = postProcessor;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();

            DynamicIssue(command);
            postProcessor?.Execute(context.Environment);

            await Task.FromResult(0);
        }

        private void DynamicIssue(object command)
        {
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => Issue(x);
                var methodCallExpression = (MethodCallExpression)expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                methodInfo.Invoke(this, new[] { command });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        private void Issue<T>(T command)
        {
            commandBus.Issue(command);
        }
    }
}