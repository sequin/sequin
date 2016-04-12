namespace Sequin.Owin
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using CommandBus;
    using Core.Infrastructure;
    using Extensions;
    using Microsoft.Owin;

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

            await DynamicIssue(command);
            postProcessor?.Execute(context.Environment);

            await Task.FromResult(0);
        }

        private Task DynamicIssue(object command)
        {
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => Issue(x);
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

        private Task Issue<T>(T command)
        {
            return commandBus.Issue(command);
        }
    }
}