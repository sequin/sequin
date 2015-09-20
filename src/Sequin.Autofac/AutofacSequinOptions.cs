namespace Sequin.Autofac
{
    using Core.Infrastructure;
    using global::Autofac;

    public class AutofacSequinOptions : SequinOptions
    {
        public AutofacSequinOptions(IComponentContext context)
        {
            HandlerFactory = new AutofacHandlerFactory(context);
        }

        public new IHandlerFactory HandlerFactory { get; }
    }
}