namespace Sequin.Integration
{
    using Fakes;
    using Microsoft.Owin.Testing;
    using Owin;
    using Xbehave;

    public abstract class FeatureBase
    {
        protected SequinOptions Options { get; set; }
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            TrackedCommandHandler.Reset();

            Server = TestServer.Create(app =>
                                       {
                                           app.UseSequin(Options ?? new OwinSequinOptions());
                                       });
        }
    }
}