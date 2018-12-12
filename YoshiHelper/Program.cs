using System;

namespace YoshiHelper
{
    class Program
    {
        static void Main(string[] args)
        {

            WelcomeUser();
            AskUserForDefaultSettings();
            
        }

        private static void AskUserForDefaultSettings()
        {
            Bus bus = new Bus();
            Console.Write("Enter start bus station: ");
            bus.startStation = Console.ReadLine();

            Console.Write("Enter end bus station: ");
            bus.endStation = Console.ReadLine();

        }

        private static void WelcomeUser()
        {
            Console.WriteLine("Welcome to our app YoshiHelper!");
            Console.WriteLine("How can I help you today?");
        }
    }
}
