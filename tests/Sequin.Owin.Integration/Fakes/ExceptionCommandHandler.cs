namespace Sequin.Owin.Integration.Fakes
{
    using System;
    using System.Threading.Tasks;

    internal class ExceptionCommandHandler : IHandler<ExceptionCommand>
    {
        public Task Handle(ExceptionCommand command)
        {
            throw new Exception("Exception command");
        }
    }
}