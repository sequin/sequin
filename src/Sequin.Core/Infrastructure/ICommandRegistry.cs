namespace Sequin.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public interface ICommandRegistry
    {
        IEnumerable<Type> GetAll();
        Type GetCommandType(string name);
    }
}