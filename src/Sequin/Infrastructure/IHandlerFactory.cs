namespace Sequin.Core.Infrastructure
{
    using System.Collections.Generic;

    public interface IHandlerFactory
    {
        ICollection<IHandler<T>> GetForCommand<T>();
    }
}