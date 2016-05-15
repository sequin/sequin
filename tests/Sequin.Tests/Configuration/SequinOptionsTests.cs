namespace Sequin.Tests.Configuration
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
        [Fact]
        public void HasDefaultCommandPath()
        {
            var options = SequinOptions.Configure().Build();
            options.CommandPath.Should().Be("/commands");
        }

        [Fact]
        public void HasDefaultCommandRegistry()
        {
            var options = SequinOptions.Configure().Build();
            options.CommandRegistry.Should().NotBeNull();
        }

        [Fact]
        public void HasDefaultHandlerFactory()
        {
            var options = SequinOptions.Configure().Build();
            options.HandlerFactory.Should().NotBeNull();
        }

        [Theory, InlineData("/test", "/test"), InlineData("test", "/test")]
        public void SetsCommandPath(string inputCommandPath, string outputCommandPath)
        {
            var options = SequinOptions.Configure()
                                       .WithCommandPath(inputCommandPath)
                                       .Build();

            options.CommandPath.Should().Be(outputCommandPath);
        }

        [Theory, InlineData(null), InlineData(""), InlineData(" ")]
        public void ThrowsExceptionIfCommandPathIsEmptyString(string commandPath)
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithCommandPath(commandPath);

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void SetsCommandRegistry()
        {
            var commandRegistry = new TestCommandRegistry();
            var options = SequinOptions.Configure()
                                       .WithCommandRegistry(commandRegistry)
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
            var options = SequinOptions.Configure()
                                       .WithHandlerFactory(handlerFactory)
                                       .Build();

            options.HandlerFactory.Should().Be(handlerFactory);
        }

        [Fact]
        public void ThrowsExceptionIfHandlerFactoryIsNull()
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithHandlerFactory(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsCommandNameResolver()
        {
            var commandNameResolver = new TestCommandNameResolver();
            var options = SequinOptions.Configure()
                                       .WithCommandNameResolver(commandNameResolver)
                                       .Build();

            options.CommandNameResolver.Should().Be(commandNameResolver);
        }

        [Fact]
        public void ThrowsExceptionIfCommandNameResolverIsNull()
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithCommandNameResolver(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SetsCommandFactory()
        {
            var commandFactory = new TestCommandFactory();
            var options = SequinOptions.Configure()
                                       .WithCommandFactory(commandFactory)
                                       .Build();

            options.CommandFactory.Should().Be(commandFactory);
        }

        [Fact]
        public void ThrowsExceptionIfCommandFactoryIsNull()
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithCommandFactory(null);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConfiguresPipeline()
        {
            var options = SequinOptions.Configure()
                                       .WithPipeline(x => new TestCommandPipelineStage
                                       {
                                           Next = x.IssueCommand
                                       })
                                       .Build();

            options.CommandPipeline.Root.Should().BeOfType<TestCommandPipelineStage>();
        }

        [Fact]
        public void ThrowsExceptionIfIssueCommandIsNotInPipeline()
        {
            var options = SequinOptions.Configure();
            Action action = () => options.WithPipeline(x => new TestCommandPipelineStage()).Build();

            action.ShouldThrow<MissingIssueCommandStageException>();
        }

        [Fact]
        public void SetsPostProcessorPipeline()
        {
            var options = SequinOptions.Configure()
                                       .WithPostProcessPipeline(new TestCommandPipelineStage())
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
