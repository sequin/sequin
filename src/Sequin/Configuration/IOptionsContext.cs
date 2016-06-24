namespace Sequin.Configuration
{
    using System;
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public interface IOptionsContext
    {
        ICommandRegistry CommandRegistry { get; }
        IHandlerFactory HandlerFactory { get; }
        Func<CommandPipeline, CommandPipelineStage> ConfigurePipeline { get; }
        CommandPipelineStage PostProcessPipelineStage { get; }
    }
}