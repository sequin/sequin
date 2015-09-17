namespace Sample.Web.Infrastructure
{
    using Autofac;
    using Sequin.Core.Infrastructure;

    internal class AutofacTypeResolver : ITypeResolver
    {
        private readonly IComponentContext _context;

        public AutofacTypeResolver(IComponentContext context)
        {
            _context = context;
        }

        public T Get<T>()
        {
            return _context.Resolve<T>();
        }
    }
}