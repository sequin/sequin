namespace Sample.Web.Autofac
{
    using System.Reflection;
    using global::Autofac;
    using Owin;
    using Sequin;
    using Sequin.Autofac;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureAutofac();
            app.UseSequin(new AutofacSequinOptions(container));
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