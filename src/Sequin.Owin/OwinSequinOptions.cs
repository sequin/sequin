namespace Sequin.Owin
{
    using Infrastructure;

    public class OwinSequinOptions : SequinOptions
    {
        public OwinSequinOptions()
        {
            CommandNameResolver = new RequestHeaderCommandNameResolver();
            CommandFactory = new JsonDeserializerCommandFactory();
        }
    }
}