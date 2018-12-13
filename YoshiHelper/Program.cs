using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoshiHelper
{
    public class Program

    {
        static void Main(string[] args)
        {

            WelcomeUser();
            Bus bus = AskUserForDefaultSettings();
            Console.Clear();

            List<DepartureTime> timeTable = ReadTimetable();
            TimeSpan targetTime = DateTime.Now.TimeOfDay;

            TimeSpan nextBus = FindNextBus(timeTable, bus.StartStation, targetTime);

            //CalculateWalkingTime(busInfo);
            TimeSpan timeUntilBusDeparture = CalculateTimeUntilBusDeparture(nextBus);

            DisplayTimeUntilBusDeparture(timeUntilBusDeparture);
            CountDown(timeUntilBusDeparture);
            
        }



        private static void WelcomeUser()
        {
            Header();
            Console.WriteLine("\nInnan Yoshi kan hjälpa till behöver vi veta lite information om dig:\n");
        }

        private static Bus AskUserForDefaultSettings()
        {
            Bus bus = new Bus();

            Console.Write("Vilken busshållplats åker du ifrån på morgonen? ");
            bus.StartStation = Console.ReadLine();

            Console.Write("Hur långt är det till busshållplatsen hemifrån? (ange i m) ");
            bus.DistanceToStartStation = double.Parse(Console.ReadLine());

            Console.Write("Vilken busshållplats hoppar du av på? ");
            bus.EndStation = Console.ReadLine();

            Console.Write("Hur långt är det från denna hållplats till jobbet? (ange i m) ");
            bus.DistanceToEndStation = double.Parse(Console.ReadLine());

            return bus;

        }

        private static List<DepartureTime> ReadTimetable()
        {
            string[] timeTable = File.ReadAllLines(@"C:\Project\YoshiHelper\YoshiHelper\Gråbosnabben.txt");
            //Det viktiga är att kunna komma åt tiderna baserat på vilken busstation man har skrivit in 

            //Den stora foreach-loopen kommer skapa 3 instanser av DepartureTime-klassen
            List<DepartureTime> departureTime = new List<DepartureTime>();

            foreach (var item in timeTable)
            {
                string[] splittedItems = item.Split(' ');
                var busStop = new DepartureTime();
                busStop.StationName = splittedItems[0];

                List<TimeSpan> times = new List<TimeSpan>();
                foreach (var time in splittedItems.Skip(1))
                {
                    TimeSpan x = TimeSpan.Parse(time);
        
                    times.Add(x);
                }
                busStop.DepartureTimes = times;
                departureTime.Add(busStop);

            }
            return departureTime;
        }

        public static TimeSpan FindNextBus(List<DepartureTime> timeTable, string stationName, TimeSpan time)
        {
            TimeSpan closestTime = new TimeSpan();

            foreach (var item in timeTable)
            {
                if (item.StationName == stationName) //här kan vi använda user input ist
                {
                    //TimeSpan targetTime = DateTime.Now.TimeOfDay;
                    //TimeSpan xxx = DateTime.Parse("18:45:00").TimeOfDay;

                    closestTime = item.DepartureTimes.Where(t => t > time).OrderBy(t => Math.Abs((t - time).Ticks)).First();
                    return closestTime;
                }
            }
            return closestTime;
        }

        private static void CalculateWalkingTime(List<Bus> busInfo)
        {
            double walkSpeed = 1.4;

            var walktoStartStationMinutes = busInfo.Select(x => x.DistanceToStartStation / walkSpeed / 60);
            Console.WriteLine($"Det tar {string.Join(",",walktoStartStationMinutes):#.#} minuter att gå till bussen hemifrån");

            var walktoEndStationMinutes = busInfo.Select(x => x.DistanceToEndStation / walkSpeed / 60);
            Console.WriteLine($"Det tar {string.Join(",", walktoEndStationMinutes):#.#} minuter att gå till bussen från jobbet");
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(TimeSpan nextBus)
        {
            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = nextBus - time.TimeOfDay; //Genom att använda time.TimeOfDay så kan time omvandlas till TimeSpan, går inte att subtrahera DateTime från TimeSpan
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

        private static void CountDown(TimeSpan timeUntilBusDeparture)
        {
            // 123456s  / 3600 = timmar

            // 80sekunder / 60 =1min
            // 80 % 60 =       20sek

            //var onesecond = new TimeSpan(0, 0, 1);

            //var t = new TimeSpan(1, 2, 0);
            //t = t.Subtract(onesecond);
            //Console.WriteLine(t.ToString());

            Console.Write("Minuter kvar till bussen går: ");
            for (int a = 600; a >= 0; a--) // a = 10 symboliserar 10 min kvar tills bussen går
            {
                Console.CursorLeft = 29;
                Console.Write(" {0:00} ", a);
                System.Threading.Thread.Sleep(1000);
            }
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
