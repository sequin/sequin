namespace Sequin.Core.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;

    public class ExclusiveHandlerCommandBus
    {
        private readonly IHandlerResolver handlerResolver;

        public ExclusiveHandlerCommandBus(IHandlerResolver handlerResolver)
        {
            this.handlerResolver = handlerResolver;
        }

        public void Issue<T>(T command)
        {
            GetHandler<T>().Handle(command);
        }

        private IHandler<T> GetHandler<T>()
        {
            var handlers = handlerResolver.GetForCommand<T>();

            if (handlers.Count == 0)
            {
                throw new CommandHandlerNotFoundException(typeof(T));
            }

            if (handlers.Count > 1)
            {
                throw new NonExclusiveCommandHandlerException(typeof(T), handlers.Select(x => x.GetType()));
            }

            var handler = handlers.Single();
            return handler;
        }
    }
}
