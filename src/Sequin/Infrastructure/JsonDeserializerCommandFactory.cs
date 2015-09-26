namespace Sequin.Infrastructure
{
    using System;
    using System.IO;
    using Core.Infrastructure;
    using Newtonsoft.Json;

    public class JsonDeserializerCommandFactory : ICommandFactory
    {
        public object Create(Type commandType, Stream requestBodyStream)
        {
            using (var streamReader = new StreamReader(requestBodyStream))
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