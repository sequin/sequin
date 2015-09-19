namespace Sequin.Autofac
{
    using Core.Infrastructure;
    using global::Autofac;

    public class AutofacSequinOptions : SequinOptions
    {
        public AutofacSequinOptions(IComponentContext context)
        {
            HandlerResolver = new AutofacHandlerResolver(context);
        }

        public new IHandlerResolver HandlerResolver { get; }
    }
}