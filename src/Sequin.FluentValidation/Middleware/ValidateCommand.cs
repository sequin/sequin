namespace Sequin.FluentValidation.Middleware
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Extensions;
    using global::FluentValidation;
    using Microsoft.Owin;

    public class ValidateCommand : OwinMiddleware
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory) : base(next)
        {
            this.validatorFactory = validatorFactory;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => Validate(context, x);
                var methodCallExpression = (MethodCallExpression) expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                await Task.Run(() => methodInfo.Invoke(this, new[] {context, command}));

            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        private async Task Validate<T>(IOwinContext context, T command)
        {
            var validator = validatorFactory.GetValidator<T>();

            if (validator == null)
            { 
                context.Response.BadRequest("No command validator registered for the specified command type.");
                return;
            }

            var result = validator.Validate(command);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(k => k.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage));

                context.Response.BadRequest("The command contained validation errors.");
                context.Response.Json(errors);
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}