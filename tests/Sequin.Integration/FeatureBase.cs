namespace Sequin.Integration
{
    using Fakes;
    using Microsoft.Owin.Testing;
    using Xbehave;

    public abstract class FeatureBase
    {
        protected SequinOptions Options { get; set; }
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            // TODO: Think of something less hacky...
            // Verifying commands were called may be easier once post-execution steps are implemented
            TrackedCommandHandler.Reset();

            Server = TestServer.Create(app =>
                                       {
                                           app.UseSequin(Options ?? new SequinOptions());
                                       });
        }
    }
}