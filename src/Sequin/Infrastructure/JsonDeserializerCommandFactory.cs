namespace Sequin.Infrastructure
{
    using System;
    using System.IO;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public class JsonDeserializerCommandFactory : ICommandFactory
    {
        public object Create(Type commandType, IOwinRequest request)
        {
            using (var streamReader = new StreamReader(request.Body))
            {
                var requestBody = streamReader.ReadToEnd();
                try
                {
                    var command = Convert.ChangeType(JsonConvert.DeserializeObject(requestBody, commandType), commandType);
                    return command;
                }
                catch (JsonReaderException ex)
                {
                    throw new CommandConstructionException(commandType, requestBody, ex);
                }
            }
        }
    }
}