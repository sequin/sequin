namespace Sequin.Integration.Configuration
{
    using System;
    using System.Collections.Generic;
    using Discovery;
    using FluentAssertions;
    using Infrastructure;
    using Sequin.Configuration;
    using Xunit;

    public class HttpOptionsTests
    {
        private readonly HttpOptionsBuilder defaultOptions;

        public HttpOptionsTests()
        {
            defaultOptions = Options.Configure()
                                          .WithHandlerFactory(x => new TestHandlerFactory())
                                          .WithCommandRegistry(x => new TestCommandRegistry())
                                          .ForHttp()
                                          .WithCommandNameResolver(x => new TestCommandNameResolver())
                                          .WithCommandFactory(x => new TestCommandFactory());
        }

        [Theory, InlineData("/test", "/test"), InlineData("test", "/test")]
        public void SetsCommandPath(string inputCommandPath, string outputCommandPath)
        {
            var options = (HttpOptions) defaultOptions.WithCommandPath(x => inputCommandPath)
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
        public void SetsCommandNameResolver()
        {
            var commandNameResolver = new TestCommandNameResolver();
            var options = (HttpOptions) defaultOptions.WithCommandNameResolver(x => commandNameResolver)
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
            var options = (HttpOptions) defaultOptions.WithCommandFactory(x => commandFactory)
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
            public TestCommandFactory() : base(new TestCommandRegistry()) { }

            protected override object Create(Type commandType)
            {
                throw new NotImplementedException();
            }
        }
    }
}
