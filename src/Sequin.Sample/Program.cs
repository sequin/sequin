namespace Sequin.Sample
{
    using System;
    using Microsoft.Owin.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:8008/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Sample running at {baseAddress}. Press enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
