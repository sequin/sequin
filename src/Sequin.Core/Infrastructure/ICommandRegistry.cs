namespace Sequin.Core.Infrastructure
{
    using System;

    public interface ICommandRegistry
    {
        Type GetCommandType(string name);
    }
}