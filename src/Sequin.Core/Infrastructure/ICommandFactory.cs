namespace Sequin.Core.Infrastructure
{
    using System;
    using System.IO;

    public interface ICommandFactory
    {
        object Create(Type commandType, Stream requestBodyStream);
    }
}