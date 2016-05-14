namespace Sequin.Owin.Discovery
{
    using System.IO;
    using global::Owin;
    using Microsoft.Owin;
    using Sequin.Discovery;

    public class OwinEnvironmentBodyProvider : ICommandBodyProvider
    {
        public string Get()
        {
            var context = new OwinContext(OwinRequestScopeContext.Current.Environment);

            using (var streamReader = new StreamReader(context.Request.Body))
            {
                var requestBody = streamReader.ReadToEnd();
                return requestBody;
            }
        }
    }
}
