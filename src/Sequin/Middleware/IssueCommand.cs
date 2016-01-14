namespace Sequin.Middleware
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using CommandBus;
    using Microsoft.Owin;
    using Extensions;

    internal class IssueCommand : OwinMiddleware
    {
        private readonly ExclusiveHandlerCommandBus commandBus;

        public IssueCommand(OwinMiddleware next, ExclusiveHandlerCommandBus commandBus) : base(next)
        {
            this.commandBus = commandBus;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => Issue(x);
                var methodCallExpression = (MethodCallExpression) expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                methodInfo.Invoke(this, new[] {command});
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            await Task.FromResult(0);
        }

        private void Issue<T>(T command)
        {
            commandBus.Issue(command);
        }
    }
}