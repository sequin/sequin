namespace Sample.Web
{
    using Owin;
    using Sequin;
    using Sequin.Documentation;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin();
            app.UseSequinDocumentation();
        }
    }
}