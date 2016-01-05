namespace Sequin.Integration.Fakes
{
    using System;
    using Core;

    internal class ExceptionCommandHandler : IHandler<ExceptionCommand>
    {
        public void Handle(ExceptionCommand command)
        {
            throw new Exception("Exception command");
        }
    }
}