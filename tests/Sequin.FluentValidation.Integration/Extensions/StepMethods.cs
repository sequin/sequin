namespace Sequin.FluentValidation.Integration.Extensions
{
    using System;
    using Xbehave;
    using Xbehave.Sdk;

    internal static class StepMethods
    {
        public static IStepBuilder Given(this string text, Action body)
        {
            return text.x(body);
        }

        public static IStepBuilder When(this string text, Action body)
        {
            return text.x(body);
        }

        public static IStepBuilder Then(this string text, Action body)
        {
            return text.x(body);
        }

        public static IStepBuilder And(this string text, Action body)
        {
            return text.x(body);
        }
    }
}