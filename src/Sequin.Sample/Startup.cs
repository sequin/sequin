namespace Sequin.Sample
{
    using global::Owin;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin();
        }
    }
}