namespace Sequin.Owin.Integration
{
    using Configuration;
    using Fakes;
    using Microsoft.Owin.Testing;
    using Xbehave;

    public abstract class FeatureBase
    {
        protected Options Options { get; set; }
        protected TestServer Server { get; private set; }

        [Background]
        public void Background()
        {
            TrackedCommandHandler.Reset();

            Server = TestServer.Create(app =>
                                       {
                                           if (Options != null)
                                           {
                                               app.UseSequin((HttpOptions)Options);
                                           }
                                           else
                                           {
                                               app.UseSequin();
                                           }
                                       });
        }
    }
}