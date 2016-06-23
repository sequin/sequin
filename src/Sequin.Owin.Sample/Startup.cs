namespace Sequin.Owin.Sample
{
    using global::Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin();
        }
    }
}