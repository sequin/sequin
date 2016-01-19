namespace Sequin.Integration.Fakes
{
    using System;
    using System.Threading.Tasks;
    using Core;

    internal class ExceptionCommandHandler : IHandler<ExceptionCommand>
    {
        public Task Handle(ExceptionCommand command)
        {
            throw new Exception("Exception command");
        }
    }
}