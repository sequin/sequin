namespace Sample.Web.Autofac
{
    using System.Reflection;
    using Infrastructure;
    using global::Autofac;
    using Owin;
    using Sequin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureAutofac();
            var typeResolver = new AutofacTypeResolver(container);

            app.UseSequin(new SequinOptions
            {
                HandlerResolver = typeResolver
            });
        }

        private static IContainer ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Handler"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            var container = builder.Build();
            return container;
        }
    }
}