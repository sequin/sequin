namespace Sequin.Autofac
{
    using global::Autofac;

    public class AutofacSequinOptions : SequinOptions
    {
        public AutofacSequinOptions(IComponentContext context)
        {
            HandlerFactory = new AutofacHandlerFactory(context);
        }
    }
}