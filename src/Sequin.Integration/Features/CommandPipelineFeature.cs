namespace Sequin.Integration.Features
{
    using System;
    using System.Reflection;
    using CommandBus;
    using Discovery;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Infrastructure;
    using Pipeline;
    using Xbehave;

    public class CommandPipelineFeature : FeatureBase
    {
        private CommandPipeline pipeline;

        [Background]
        public void FeatureBackground()
        {
            pipeline = new CommandPipeline(new ExclusiveHandlerCommandBus(new ReflectionHandlerFactory(Assembly.GetAssembly(typeof(TrackedCommand)))));
        }

        [Scenario]
        public void IssuesCommand(TrackedCommand command)
        {
            "Given I have a command"
                .Given(() =>
                {
                    command = new TrackedCommand { A = 1, B = 2 };
                });

            "When the command is processed in the pipeline"
                .When(async () =>
                {
                    await pipeline.Execute(command);
                });

            "Then the command handler should have been executed"
                .Then(() =>
                {
                    TrackedCommandHandler.LastCommand.Should().BeSameAs(command);
                });
        }

        [Scenario]
        public void MultiplePipelineStages(TrackedCommand command, TrackedPipelineStage stageA, TrackedPipelineStage stageB)
        {
            "Given I have a command"
                .Given(() =>
                {
                    command = new TrackedCommand { A = 1, B = 2 };
                });

            "And multiple stages in the pipeline which the command can passthrough"
                .And(() =>
                {
                    stageA = new TrackedPipelineStage();
                    stageB = new TrackedPipelineStage();

                    stageA.Next = stageB;
                    stageB.Next = pipeline.IssueCommand;

                    pipeline.SetRoot(stageA);
                });

            "When the command is processed in the pipeline"
                .When(async () =>
                {
                    await pipeline.Execute(command);
                });

            "Then the command handler should have been executed"
                .Then(() =>
                {
                    TrackedCommandHandler.LastCommand.Should().BeSameAs(command);
                });

            "And each additional pipeline stage should have been executed"
                .And(() =>
                {
                    stageA.LastCommand.Should().BeSameAs(command);
                    stageB.LastCommand.Should().BeSameAs(command);
                });
        }

        [Scenario]
        public void StopPipeline(TrackedCommand command, BlockingPipelineStage stageA, TrackedPipelineStage stageB)
        {
            "Given I have a command"
                .Given(() =>
                {
                    command = new TrackedCommand { A = 1, B = 2 };
                });

            "And a stage in the pipeline which the command cannot passthrough"
                .And(() =>
                {
                    stageA = new BlockingPipelineStage { Next = pipeline.IssueCommand };
                    pipeline.SetRoot(stageA);
                });

            "And additional stages in the pipeline which the command can passthrough"
                .And(() =>
                {
                    stageB = new TrackedPipelineStage();

                    stageA.Next = stageB;
                    stageB.Next = pipeline.IssueCommand;
                });

            "When the command is processed in the pipeline"
                .When(async () =>
                {
                    await pipeline.Execute(command);
                });

            "Then the command handler should not have been executed"
                .Then(() =>
                {
                    TrackedCommandHandler.HasExecuted.Should().BeFalse();
                });

            "And the blocking pipeline stage should have been executed"
                .And(() =>
                {
                    stageA.HasExecuted.Should().BeTrue();
                });

            "And each additional pipeline stage should not have been executed"
                .And(() =>
                {
                    stageB.HasExecuted.Should().BeFalse();
                });
        }

        [Scenario]
        public void PostProcessingCommand(TrackedCommand command, TrackedPipelineStage postProcessStage)
        {
            "Given I have a command"
                .Given(() =>
                {
                    command = new TrackedCommand { A = 1, B = 2 };
                });

            "And I have a pipeline stage configured after the command is handled"
                .And(() =>
                {
                    postProcessStage = new TrackedPipelineStage();
                    pipeline.IssueCommand.Next = postProcessStage;
                });

            "When the command is processed in the pipeline"
                .When(async () =>
                {
                    await pipeline.Execute(command);
                });

            "Then the command handler should have been executed"
                .Then(() =>
                {
                    TrackedCommandHandler.LastCommand.Should().BeSameAs(command);
                });

            "And the extra pipeline stage should have been executed"
                .And(() =>
                {
                    postProcessStage.LastCommand.Should().BeSameAs(command);
                });
        }

        [Scenario]
        public void ExceptionInStage(TrackedCommand command, Exception exception)
        {
            "Given I have a command"
                .Given(() =>
                {
                    command = new TrackedCommand { A = 1, B = 2 };
                });

            "And a pipeline stage which causes an exception"
                .And(() =>
                {
                    var stageA = new ExceptionPipelineStage {Next = pipeline.IssueCommand};
                    pipeline.SetRoot(stageA);
                });

            "When the command is processed in the pipeline"
                .When(async () =>
                {
                    try
                    {
                        await pipeline.Execute(command);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                });

            "Then the exception is not captured"
                .Then(() =>
                {
                    exception.Should().NotBeNull();
                });
        }
    }
}
