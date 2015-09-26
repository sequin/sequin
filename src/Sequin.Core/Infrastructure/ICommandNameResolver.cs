namespace Sequin.Core.Infrastructure
{
    using System.Collections.Generic;

    public interface ICommandNameResolver
    {
        string GetCommandName(IDictionary<string, object> environment);
    }
}