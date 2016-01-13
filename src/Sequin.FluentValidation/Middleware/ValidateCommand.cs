namespace Sequin.FluentValidation.Middleware
{
    using System;
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
        private readonly IValidationResultFormatter validationResultFormatter;

        public ValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory) : this(next, validatorFactory, new DefaultDictionaryFormatter()) { }

        public ValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory, IValidationResultFormatter validationResultFormatter) : base(next)
        {
            this.validatorFactory = validatorFactory;
            this.validationResultFormatter = validationResultFormatter;
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

        private void WriteErrorResponse(IOwinContext context, ValidationResult validationResult)
        {
            var errors = validationResultFormatter.Format(validationResult);
            
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