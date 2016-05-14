namespace Sequin.Owin
{
    using Discovery;
    using Sequin.Discovery;

    public class OwinSequinOptions : SequinOptions
    {
        public OwinSequinOptions()
        {
            CommandNameResolver = new RequestHeaderCommandNameResolver();
            CommandFactory = new JsonDeserializerCommandFactory(CommandRegistry, new OwinEnvironmentBodyProvider());
        }
    }
}