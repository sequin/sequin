namespace Sequin.CommandBus
{
    using System.Linq;
    using Core;
    using Core.Infrastructure;

    public class ExclusiveHandlerCommandBus
    {
        private readonly IHandlerFactory handlerFactory;

        public ExclusiveHandlerCommandBus(IHandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public void Issue<T>(T command)
        {
            GetHandler<T>().Handle(command);
        }

        private IHandler<T> GetHandler<T>()
        {
            var handlers = handlerFactory.GetForCommand<T>();

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
