namespace Sequin.Autofac
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Infrastructure;
    using global::Autofac;

    internal class AutofacHandlerResolver : IHandlerResolver
    {
        private readonly IComponentContext context;

        public AutofacHandlerResolver(IComponentContext context)
        {
            this.context = context;
        }

        ICollection<IHandler<T>> IHandlerResolver.GetForCommand<T>()
        {
            return context.Resolve<IEnumerable<IHandler<T>>>().ToList();
        }
    }
}