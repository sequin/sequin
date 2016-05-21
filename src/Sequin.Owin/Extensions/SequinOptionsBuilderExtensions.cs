namespace Sequin.Owin.Extensions
{
    using Configuration;
    using Discovery;
    using Sequin.Discovery;

    public static class SequinOptionsBuilderExtensions
    {
        public static SequinOptionsBuilder WithOwinDefaults(this SequinOptionsBuilder builder)
        {
            return builder.WithCommandNameResolver(x => new RequestHeaderCommandNameResolver())
                          .WithCommandFactory(x => new JsonDeserializerCommandFactory(x.CommandRegistry, new OwinEnvironmentBodyProvider()));
        }
    }
}
