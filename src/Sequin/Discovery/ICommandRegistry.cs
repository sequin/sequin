namespace Sequin.Discovery
{
    using System;

    public interface ICommandRegistry
    {
        Type GetCommandType(string name);
    }
}