namespace Sequin.Core.Infrastructure
{
    using System.Collections.Generic;

    public interface ICommandPostProcessor
    {
        void Execute(IDictionary<string, object> environment);
    }
}