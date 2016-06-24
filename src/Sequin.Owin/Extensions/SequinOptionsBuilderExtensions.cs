namespace Sequin.Owin.Extensions
{
    using Configuration;
    using Discovery;
    using Sequin.Discovery;

    public static class SequinOptionsBuilderExtensions
    {
        public static HttpOptionsBuilder WithOwinDefaults(this HttpOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.WithCommandNameResolver(x => new RequestHeaderCommandNameResolver())
                                 .WithCommandFactory(x => new JsonDeserializerCommandFactory(x.CommandRegistry, new OwinEnvironmentBodyProvider()));
        }
    }
}
