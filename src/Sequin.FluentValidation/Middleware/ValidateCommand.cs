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
    using global::FluentValidation.Results;
    using Microsoft.Owin;

    public class ValidateCommand : OwinMiddleware
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory) : base(next)
        {
            this.validatorFactory = validatorFactory;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();
            var validationResult = Validate(command);

            if (validationResult != null && validationResult.IsValid)
            {
                await Next.Invoke(context);
            }
            else if (validationResult != null)
            {
                WriteErrorResponse(context, validationResult);
            }
            else
            {
                context.Response.BadRequest("No command validator registered for the specified command type.");
            }
        }

        private static void WriteErrorResponse(IOwinContext context, ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                                         .GroupBy(k => k.PropertyName)
                                         .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage));

            context.Response.BadRequest("The command contained validation errors.");
            context.Response.Json(errors);
        }

        private ValidationResult Validate(object command)
        {
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => ExecuteValidator(x);
                var methodCallExpression = (MethodCallExpression)expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                var result = (ValidationResult)methodInfo.Invoke(this, new[] { command });
                return result;

            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return null;
        }

        private ValidationResult ExecuteValidator<T>(T command)
        {
            var validator = validatorFactory.GetValidator<T>();
            var result = validator?.Validate(command);

            return result;
        }
    }
}