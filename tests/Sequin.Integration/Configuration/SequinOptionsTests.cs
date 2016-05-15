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

    public class SequinOptionsTests
    {
        private readonly SequinOptionsBuilder defaultOptions;

        public SequinOptionsTests()
        {
            defaultOptions = SequinOptions.Configure()
                                          .WithCommandNameResolver(new TestCommandNameResolver())
                                          .WithHandlerFactory(new TestHandlerFactory())
                                          .WithCommandRegistry(new TestCommandRegistry())
                                          .WithCommandFactory(x => new TestCommandFactory());
        }

        [Theory, InlineData("/test", "/test"), InlineData("test", "/test")]
        public void SetsCommandPath(string inputCommandPath, string outputCommandPath)
        {
            var options = defaultOptions.WithCommandPath(inputCommandPath)
                                        .Build();

            options.CommandPath.Should().Be(outputCommandPath);
        }

        [Theory, InlineData(null), InlineData(""), InlineData(" ")]
        public void ThrowsExceptionIfCommandPathIsEmptyString(string commandPath)
        {
            Action action = () => defaultOptions.WithCommandPath(commandPath);
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void SetsCommandRegistry()
        {
            var commandRegistry = new TestCommandRegistry();
            var options = defaultOptions.WithCommandRegistry(commandRegistry)
                                        .Build();

            options.CommandRegistry.Should().Be(commandRegistry);
        }

        [Fact]
        public void ThrowsExceptionIfCommandRegistryIsNull()
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithCommandRegistry(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsHandlerFactory()
        {
            var handlerFactory = new TestHandlerFactory();
            var options = defaultOptions.WithHandlerFactory(handlerFactory)
                                        .Build();

            options.HandlerFactory.Should().Be(handlerFactory);
        }

        [Fact]
        public void ThrowsExceptionIfHandlerFactoryIsNull()
        {
            Action action = () => defaultOptions.WithHandlerFactory(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsCommandNameResolver()
        {
            var commandNameResolver = new TestCommandNameResolver();
            var options = defaultOptions.WithCommandNameResolver(commandNameResolver)
                                        .Build();

            options.CommandNameResolver.Should().Be(commandNameResolver);
        }

        [Fact]
        public void ThrowsExceptionIfCommandNameResolverIsNull()
        {
            Action action = () => defaultOptions.WithCommandNameResolver(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsCommandFactory()
        {
            var commandFactory = new TestCommandFactory();
            var options = defaultOptions.WithCommandFactory(x => commandFactory)
                                        .Build();

            options.CommandFactory.Should().Be(commandFactory);
        }

        [Fact]
        public void ThrowsExceptionIfCommandFactoryFuncIsNull()
        {
            Action action = () => defaultOptions.WithCommandFactory(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionIfResolvedCommandFactoryIsNull()
        {
            var options = defaultOptions.WithCommandFactory(x => null);

            Action action = () => options.Build();
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

        private class TestCommandNameResolver : ICommandNameResolver
        {
            public string GetCommandName()
            {
                throw new NotImplementedException();
            }
        }

        private class TestCommandFactory : CommandFactory
        {
            public TestCommandFactory() : base(new TestCommandRegistry()) {}

            protected override object Create(Type commandType)
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
