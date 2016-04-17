namespace Sequin.Extensions
{
    using Microsoft.Owin;

    public static class IOwinContextExtensions
    {
        public static object GetCommand(this IOwinContext context)
        {
            return context.Get<object>("commands.Command");
        }

        public static void SetCommand(this IOwinContext context, object command)
        {
            context.Set("commands.Command", command);
        }
    }
}