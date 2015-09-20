namespace Sample.Web.FluentValidation
{
    using Owin;
    using Sequin;
    using Sequin.FluentValidation.Middleware;
    using Sequin.Infrastructure;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseSequin(new SequinOptions
            {
                CommandPipeline = new []
                {
                    new CommandPipelineStage(typeof(ValidateCommand), new LazyValidatorFactory()), 
                }
            });
        }
    }
}