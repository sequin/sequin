namespace Sequin.Sample
{
    using System;
    using Commands;
    using Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var mediator = CreateMediator();

            Console.WriteLine("Sending command directly to pipeline...");

            mediator.Send(new DummyCommand {DummyProperty = 1, OtherDummyProperty = 2}).Wait();

            Console.WriteLine("Command sent");
            Console.ReadLine();
        }

        private static Mediator CreateMediator()
        {
            var options = Options.Configure().Build();
            return new Mediator(options.CommandPipeline);
        }
    }
}
