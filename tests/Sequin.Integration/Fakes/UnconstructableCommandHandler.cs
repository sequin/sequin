namespace Sequin.Integration.Fakes
{
    using Core;

    public class UnconstructableCommandHandler : IHandler<UnconstructableCommandHandlerTest>
    {
        public UnconstructableCommandHandler(object obj)
        {
            // No parameterless constructor to make handler unconstructable with default factory
        }

        public void Handle(UnconstructableCommandHandlerTest command)
        {
            // Do nothing
        }
    }

    public class UnconstructableCommandHandlerTest { }
}