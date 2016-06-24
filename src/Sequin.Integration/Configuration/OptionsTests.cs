namespace Sequin.Integration.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discovery;
    using FluentAssertions;
    using Infrastructure;
    using Pipeline;
    using Sequin.Configuration;
    using Xunit;

    public class OptionsTests
    {
        private readonly OptionsBuilder defaultOptions;

        public OptionsTests()
        {
            defaultOptions = Options.Configure()
                                          .WithHandlerFactory(x => new TestHandlerFactory())
                                          .WithCommandRegistry(x => new TestCommandRegistry());
        }

        [Fact]
        public void SetsCommandRegistry()
        {
            var commandRegistry = new TestCommandRegistry();
            var options = defaultOptions.WithCommandRegistry(x => commandRegistry)
                                        .Build();

            options.CommandRegistry.Should().Be(commandRegistry);
        }

        [Fact]
        public void ThrowsExceptionIfCommandRegistryIsNull()
        {
            Action action = () => defaultOptions.WithCommandRegistry(x => null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionIfCommandRegistryResolverIsNull()
        {
            Action action = () => defaultOptions.WithCommandRegistry(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsHandlerFactory()
        {
            var handlerFactory = new TestHandlerFactory();
            var options = defaultOptions.WithHandlerFactory(x => handlerFactory)
                                        .Build();

            options.HandlerFactory.Should().Be(handlerFactory);
        }

        [Fact]
        public void ThrowsExceptionIfHandlerFactoryIsNull()
        {
            Action action = () => defaultOptions.WithHandlerFactory(x => null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionIfHandlerFactoryResolverIsNull()
        {
            Action action = () => defaultOptions.WithHandlerFactory(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConfiguresPipeline()
        {
            var options = defaultOptions.WithPipeline(x => new TestCommandPipelineStage
            {
                Next = x.IssueCommand
            }).Build();

            options.CommandPipeline.Root.Should().BeOfType<TestCommandPipelineStage>();
        }

        [Fact]
        public void ThrowsExceptionIfIssueCommandIsNotInPipeline()
        {
            Action action = () => defaultOptions.WithPipeline(x => new TestCommandPipelineStage()).Build();

            action.ShouldThrow<MissingIssueCommandStageException>();
        }

        [Fact]
        public void SetsPostProcessorPipeline()
        {
            var options = defaultOptions.WithPostProcessPipeline(new TestCommandPipelineStage())
                                        .Build();

            options.CommandPipeline.Root.Next.Should().BeOfType<TestCommandPipelineStage>();
        }

        private class TestCommandRegistry : ICommandRegistry
        {
            public Type GetCommandType(string name)
            {
                throw new NotImplementedException();
            }
        }

        private class TestHandlerFactory : IHandlerFactory
        {
            ICollection<IHandler<T>> IHandlerFactory.GetForCommand<T>()
            {
                throw new NotImplementedException();
            }
        }

        private class TestCommandPipelineStage : CommandPipelineStage
        {
            protected override Task Process<TCommand>(TCommand command)
            {
                throw new NotImplementedException();
            }
        }
    }
}
