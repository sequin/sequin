namespace Sequin.Configuration
{
    using Discovery;
    using Infrastructure;

    public interface ISequinOptionsContext
    {
        string CommandPath { get; }
        ICommandRegistry CommandRegistry { get; }
        IHandlerFactory HandlerFactory { get; }
        ICommandNameResolver CommandNameResolver { get; }
        CommandFactory CommandFactory { get; }
    }
}