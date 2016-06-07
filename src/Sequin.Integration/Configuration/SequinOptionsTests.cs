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
                                          .WithCommandNameResolver(x => new TestCommandNameResolver())
                                          .WithHandlerFactory(x => new TestHandlerFactory())
                                          .WithCommandRegistry(x => new TestCommandRegistry())
                                          .WithCommandFactory(x => new TestCommandFactory());
        }

        [Theory, InlineData("/test", "/test"), InlineData("test", "/test")]
        public void SetsCommandPath(string inputCommandPath, string outputCommandPath)
        {
            var options = defaultOptions.WithCommandPath(x => inputCommandPath)
                                        .Build();

            options.CommandPath.Should().Be(outputCommandPath);
        }

        [Theory, InlineData(null), InlineData(""), InlineData(" ")]
        public void ThrowsExceptionIfCommandPathIsEmptyString(string commandPath)
        {
            Action action = () => defaultOptions.WithCommandPath(x => commandPath);
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ThrowsExceptionIfCommandPathResolverIsNull()
        {
            Action action = () => defaultOptions.WithCommandPath(null);
            action.ShouldThrow<ArgumentNullException>();
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
        public void SetsCommandNameResolver()
        {
            var commandNameResolver = new TestCommandNameResolver();
            var options = defaultOptions.WithCommandNameResolver(x => commandNameResolver)
                                        .Build();

            options.CommandNameResolver.Should().Be(commandNameResolver);
        }

        [Fact]
        public void ThrowsExceptionIfCommandNameResolverIsNull()
        {
            Action action = () => defaultOptions.WithCommandNameResolver(x => null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsExceptionIfCommandNameResolverFuncIsNull()
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
        public void ThrowsExceptionIfCommandFactoryIsNull()
        {
            Action action = () => defaultOptions.WithCommandFactory(x => null);
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
