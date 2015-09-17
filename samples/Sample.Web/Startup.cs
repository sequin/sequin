namespace Sample.Web
{
    using System.Reflection;
    using Autofac;
    using Infrastructure;
    using Owin;
    using Sequin.Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureAutofac();
            var typeResolver = new AutofacTypeResolver(container);

            app.UseSequin(new SequinOptions
            {
                CommandRegistry = new ReflectionCommandRegistry(Assembly.GetExecutingAssembly()),
                TypeResolver = typeResolver
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