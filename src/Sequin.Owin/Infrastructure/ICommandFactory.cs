namespace Sequin.Owin.Infrastructure
{
    using System;
    using Microsoft.Owin;

    public interface ICommandFactory
    {
        object Create(Type commandType, IOwinRequest request);
    }
}