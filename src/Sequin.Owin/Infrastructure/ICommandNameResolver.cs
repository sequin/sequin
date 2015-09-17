namespace Sequin.Owin.Infrastructure
{
    using Microsoft.Owin;

    public interface ICommandNameResolver
    {
        string GetCommandName(IOwinRequest request);
    }
}