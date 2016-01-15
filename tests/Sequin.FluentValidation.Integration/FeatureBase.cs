namespace Sequin.FluentValidation.Integration
{
    using System;
    using Fakes;
    using Infrastructure;
    using Microsoft.Owin.Testing;
    using Middleware;
    using Xbehave;

    public abstract class FeatureBase
    {
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            // TODO: Think of something less hacky...
            // Verifying commands were called may be easier once post-execution steps are implemented
            ValidatedCommandHandler.Reset();

            Server = TestServer.Create(app =>
                                       {
                                           app.UseSequin(new SequinOptions
                                                         {
                                                            CommandPipeline = new []
                                                                              {
                                                                                  new CommandPipelineStage(typeof(ValidateCommand), new ReflectionValidatorFactory(AppDomain.CurrentDomain.GetAssemblies())), 
                                                                              }
                                                         });
                                       });
        }
    }
}