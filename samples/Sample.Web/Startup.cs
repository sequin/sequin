namespace Sample.Web
{
    using Owin;
    using Sequin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin();
        }
    }
}