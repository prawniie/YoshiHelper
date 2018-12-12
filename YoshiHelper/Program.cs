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
            Console.Clear();

            ReadTimetable();
            FindNextBus();

            CalculateWalkingTime(busInfo);
            TimeSpan timeUntilBusDeparture = CalculateTimeUntilBusDeparture(busInfo);

            DisplayTimeUntilBusDeparture(timeUntilBusDeparture);
            
        }



        private static void WelcomeUser()
        {
            Header();
            Console.WriteLine("\nInnan Yoshi kan hjälpa till behöver vi veta lite information om dig!:\n");
        }

        private static List<Bus> AskUserForDefaultSettings()
        {
            List<Bus> busInfo = new List<Bus>();
            Bus bus = new Bus();

            Console.Write("Vilken busshållplats åker du ifrån på morgonen? ");
            bus.StartStation = Console.ReadLine();

            Console.Write("Hur långt är det till busshållplatsen hemifrån? (ange i m) ");
            bus.DistanceToStartStation = double.Parse(Console.ReadLine());

            Console.Write("Vilken busshållplats hoppar du av på? ");
            bus.EndStation = Console.ReadLine();

            Console.Write("Hur långt är det från denna hållplats till jobbet? (ange i m) ");
            bus.DistanceToEndStation = double.Parse(Console.ReadLine());

            busInfo.Add(bus);
            return busInfo;

        }

        private static string[] ReadTimetable()
        {
            string[] timeTable = File.ReadAllLines(@"C:\Project\YoshiHelper\YoshiHelper\YoshiHelper\Gråbosnabben.txt");
            //Det viktiga är att kunna komma åt tiderna baserat på vilken busstation man har skrivit in 

            List<DepartureTime> departureTime = new List<DepartureTime>();

            foreach (var item in timeTable)
            {
                string[] splittedItems = item.Split(' ');
                var busStop = new DepartureTime();
                busStop.StationName = splittedItems[0];


            }

            for (int i = 0; i < timeTable.Length; i++)
            {
                string[] enskildBusshållPlats = 
            }


            List<Bus> busInfo = new List<Bus>();

            int counter = 0;
            foreach (var item in timeTable)
            {

                string[] åkarePlatsen = item;
            }
            return timeTable;
        }

        private static void FindNextBus()
        {

            ////FIND CLOSEST ELEMENT IN ARRAY; går att använda för oss där targetNumber är klockslag ?
            //int[] array = new int[5] { 5, 7, 8, 15, 20 };

            //int TargetNumber = 13;
            //var nearest = array.MinBy(x => Math.Abs((long)x - targetNumber));
        }

        private static void CalculateWalkingTime(List<Bus> busInfo)
        {
            double walkSpeed = 1.4;

            var walktoStartStationMinutes = busInfo.Select(x => x.DistanceToStartStation / walkSpeed / 60);
            Console.WriteLine($"Det tar {string.Join(",",walktoStartStationMinutes):#.#} minuter att gå till bussen hemifrån");

            var walktoEndStationMinutes = busInfo.Select(x => x.DistanceToEndStation / walkSpeed / 60);
            Console.WriteLine($"Det tar {string.Join(",", walktoEndStationMinutes):#.#} minuter att gå till bussen från jobbet");
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(List<Bus> busInfo)
        {
            Bus bus = new Bus();
            bus.StartTime = new TimeSpan(14, 50, 00);

            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = bus.StartTime - time.TimeOfDay; //Genom att använda time.TimeOfDay så kan time omvandlas till TimeSpan, går inte att subtrahera DateTime från TimeSpan
            Console.WriteLine($"\nDet är {timeUntilBusDeparture:mm} minuter kvar tills bussen går");

            return timeUntilBusDeparture;
        }

        private static void DisplayTimeUntilBusDeparture(TimeSpan timeUntilBusDeparture)
        {
            TimeSpan hurry = new TimeSpan(00, 05, 00);

            if (timeUntilBusDeparture > hurry)
                WriteGreen("Du har gott om tid!");
            else
                WriteRed("Nu har du bråttom!");

            //Lägga till hur lång tid det tar att gå till bussen, anpassa meddelandena efter det
        }

        private static void WriteGreen(string text)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void WriteRed(string text)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void Header()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("YoshiHelper".PadLeft(25));
            Console.WriteLine("---------------------------------------------------------");
            Console.ResetColor();

        }




    }
}
