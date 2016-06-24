namespace Sequin.Configuration
{
    using Discovery;

    public interface IHttpOptionsContext : IOptionsContext
    {
        string CommandPath { get; }
        ICommandNameResolver CommandNameResolver { get; }
        CommandFactory CommandFactory { get; }
    }
}