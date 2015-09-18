namespace Sequin.Core.Infrastructure
{
    using System.Collections.Generic;

    public interface IHandlerResolver
    {
        ICollection<IHandler<T>> GetForCommand<T>();
    }
}