namespace Sequin.Discovery
{
    using System;
    using Extensions;
    using Infrastructure;
    using Newtonsoft.Json;

    public class JsonDeserializerCommandFactory : CommandFactory
    {
        private readonly ICommandBodyProvider commandBodyProvider;
        private readonly JsonSerializerSettings serializerSettings;

        public JsonDeserializerCommandFactory(ICommandRegistry commandRegistry, ICommandBodyProvider commandBodyProvider) : this(commandRegistry, commandBodyProvider, new JsonSerializerSettings()) { }

        public JsonDeserializerCommandFactory(ICommandRegistry commandRegistry, ICommandBodyProvider commandBodyProvider, JsonSerializerSettings serializerSettings) : base(commandRegistry)
        {
            this.commandBodyProvider = commandBodyProvider;
            this.serializerSettings = serializerSettings;
        }

        protected override object Create(Type commandType)
        {
            var commandBody = commandBodyProvider.Get();

            try
            {
                var command = Convert.ChangeType(JsonConvert.DeserializeObject(commandBody, commandType, serializerSettings), commandType);
                return command;
            }
            catch (JsonSerializationException ex)
            {
                throw new CommandConstructionException(ex.Message, commandType, commandBody, ex);
            }
            catch (JsonReaderException ex)
            {
                throw new CommandConstructionException("JSON command body could not be read; it may be malformed.", commandType, commandBody, ex);
            }
        }
    }
}