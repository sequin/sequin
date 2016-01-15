namespace Sequin.FluentValidation.Integration.Fakes
{
    using Core;

    public class ValidatedCommandHandler : IHandler<ValidatedCommand>
    {
        public void Handle(ValidatedCommand command)
        {
            LastCommand = command;
        }

        public static ValidatedCommand LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;

        public static void Reset()
        {
            LastCommand = null;
        }
    }
}