namespace Sequin.Owin.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Owin;
    using Newtonsoft.Json;
    using Sequin.Infrastructure;

    public class JsonDeserializerCommandFactory : ICommandFactory
    {
        private readonly JsonSerializerSettings serializerSettings;

        public JsonDeserializerCommandFactory() : this(new JsonSerializerSettings()) { }

        public JsonDeserializerCommandFactory(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
        }

        public object Create(Type commandType, IDictionary<string, object> environment)
        {
            var request = new OwinRequest(environment);
            using (var streamReader = new StreamReader(request.Body))
            {
                var requestBody = streamReader.ReadToEnd();
                try
                {
                    var command = Convert.ChangeType(JsonConvert.DeserializeObject(requestBody, commandType, serializerSettings), commandType);
                    return command;
                }
                catch (JsonSerializationException ex)
                {
                    throw new CommandConstructionException(ex.Message, commandType, requestBody, ex);
                }
                catch (JsonReaderException ex)
                {
                    throw new CommandConstructionException("JSON command body could not be read; it may be malformed.", commandType, requestBody, ex);
                }
            }
        }
    }
}