namespace Sequin.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public interface ICommandFactory
    {
        object Create(Type commandType, IDictionary<string, object> environment);
    }
}