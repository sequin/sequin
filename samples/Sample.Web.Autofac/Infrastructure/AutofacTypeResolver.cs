namespace Sample.Web.Autofac.Infrastructure
{
    using global::Autofac;
    using Sequin.Core.Infrastructure;

    internal class AutofacTypeResolver : IHandlerResolver
    {
        private readonly IComponentContext context;

        public AutofacTypeResolver(IComponentContext context)
        {
            this.context = context;
        }

        public T GetForCommand<T>()
        {
            return context.Resolve<T>();
        }
    }
}