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
            ReadTimetable();

            CalculateWalkingTime(busInfo);
            TimeSpan timeUntilBusDeparture = CalculateTimeUntilBusDeparture(busInfo);

            DisplayTimeUntilBusDeparture(timeUntilBusDeparture);
            
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

            var walktoStartStationMinutes = busInfo.Select(x => x.DistanceToStartStation / walkSpeed / 60);
            Console.WriteLine($"It takes {string.Join(",",walktoStartStationMinutes):#.#} minutes to walk to the bus from home");

            var walktoEndStationMinutes = busInfo.Select(x => x.DistanceToEndStation / walkSpeed / 60);
            Console.WriteLine($"It takes {string.Join(",", walktoEndStationMinutes):#.#} minutes to walk to the bus from work");
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(List<Bus> busInfo)
        {
            Bus bus = new Bus();
            bus.StartTime = new TimeSpan(14, 30, 00);

            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = bus.StartTime - time.TimeOfDay; //Genom att använda time.TimeOfDay så kan time omvandlas till TimeSpan, går inte att subtrahera DateTime från TimeSpan
            Console.WriteLine($"Det är {timeUntilBusDeparture:mm} minuter kvar tills bussen går");

            return timeUntilBusDeparture;

        }

        private static void DisplayTimeUntilBusDeparture(TimeSpan timeUntilBusDeparture)
        {

            TimeSpan hurry = new TimeSpan(00, 05, 00);

            if (timeUntilBusDeparture > hurry)
            {
                Console.WriteLine("You have plenty of time!");
            }
            else
            {
                Console.WriteLine("You are in a hurry!");
            }
        }

        


    }
}
