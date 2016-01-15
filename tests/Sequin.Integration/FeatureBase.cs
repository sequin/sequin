namespace Sequin.Integration
{
    using Microsoft.Owin.Testing;
    using Xbehave;

    public abstract class FeatureBase
    {
        protected SequinOptions Options { get; set; }
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            Server = TestServer.Create(app =>
                                       {
                                           app.UseSequin(Options ?? new SequinOptions());
                                       });
        }
    }
}