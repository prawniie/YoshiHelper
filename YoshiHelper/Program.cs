using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoshiHelper
{
    class Program

    {
        static void Main(string[] args)
        {

            WelcomeUser();
            List<Bus> busInfo = AskUserForDefaultSettings();
            //ReadTimetable();
            CalculateWalkingTime(busInfo);
            
        }

        private static void WelcomeUser()
        {
            Console.WriteLine("Welcome to our app YoshiHelper!");
            Console.WriteLine("First we need to know some things about you:\n");
        }

        private static List<Bus> AskUserForDefaultSettings()
        {
            List<Bus> busInfo = new List<Bus>();
            Bus bus = new Bus();

            Console.Write("Enter start bus station: ");
            bus.StartStation = Console.ReadLine();

            Console.Write("Enter distance from home to start bus station (in m): ");
            bus.DistanceToStartStation = double.Parse(Console.ReadLine());

            Console.Write("Enter end bus station: ");
            bus.EndStation = Console.ReadLine();

            Console.Write("Enter distance from end bus station to work (in m): ");
            bus.DistanceToEndStation = double.Parse(Console.ReadLine());

            busInfo.Add(bus);
            return busInfo;

        }

        private static void ReadTimetable()
        {
            string[] timeTable = File.ReadAllLines(@"C:\Project\YoshiHelper\YoshiHelper\YoshiHelper\Gråbosnabben.txt");
        }

        private static void CalculateWalkingTime(List<Bus> busInfo)
        {
            double walkSpeed = 1.4;

            var walktoStartStationSeconds = busInfo.Select(x => x.DistanceToStartStation / walkSpeed / 60);
            Console.WriteLine($"It takes {string.Join(",",walktoStartStationSeconds):0.#} minutes to walk to the bus");

            //double walktoStartStationMinutes = Convert.ToDouble(walktoStartStationSeconds) / 60;

            //Console.WriteLine($"It takes {walktoStartStationMinutes:0.##} minutes to walk to the start station from home");

            //Bus bus = new Bus();
            //List<Bus> x = new List<Bus>();


            //double walkToEndStationSeconds = bus.DistanceToEndStation / walkSpeed;
            //double walkToEndStationMinutes = walkToEndStationSeconds / 60;
            //Console.WriteLine($"It takes {walkToEndStationMinutes} minutes to walk to the end station from work");


        }




    }
}
