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

            List<BusStation> timeTable = ReadTimetable();
            TimeSpan targetTime = DateTime.Now.TimeOfDay;

            TimeSpan nextBus = FindNextBus(timeTable, bus.StartStation, targetTime);

            TimeSpan walkingTimeToBus = CalculateWalkingTime(bus, bus.DistanceToStartStation);
            TimeSpan walkingTimeToWork = CalculateWalkingTime(bus, bus.DistanceToEndStation);

            TimeSpan timeUntilBusDeparture = CalculateTimeUntilBusDeparture(nextBus);//Rebecka
            TimeSpan timeUntilYouNeedToGo = CalculateTimeUntilYouNeedToGo(nextBus, walkingTimeToBus);

            CalculateBusrideTime(timeTable, bus.EndStation, nextBus); //Georg

            CountDown(timeUntilYouNeedToGo); //Rebecka
            
        }



        private static void WelcomeUser()
        {
            Header();
            Console.WriteLine("\nInnan Yoshi kan hjälpa till behöver vi veta lite information om dig:\n");
        }

        private static Bus AskUserForDefaultSettings()
        {
            Bus bus = new Bus();

            //Lägga till validering
            //Busshållplats som enum? se uppgift med Enums och sport

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

        private static List<BusStation> ReadTimetable()
        {
            string[] timeTable = File.ReadAllLines(@"C:\Project\YoshiHelper\YoshiHelper\Gråbosnabben.txt");
            //Det viktiga är att kunna komma åt tiderna baserat på vilken busstation man har skrivit in 

            //Den stora foreach-loopen kommer skapa 3 instanser av BusStation-klassen
            List<BusStation> departureTimeForAllBusStops = new List<BusStation>();

            foreach (var item in timeTable)
            {
                string[] splittedItems = item.Split(' ');
                var busStop = new BusStation();
                busStop.StationName = splittedItems[0];

                List<TimeSpan> times = new List<TimeSpan>();
                foreach (var time in splittedItems.Skip(1))
                {
                    TimeSpan t = TimeSpan.Parse(time);
        
                    times.Add(t);
                }
                busStop.DepartureTimes = times;
                departureTimeForAllBusStops.Add(busStop);

            }
            return departureTimeForAllBusStops;
        }

        public static TimeSpan FindNextBus(List<BusStation> timeTable, string stationName, TimeSpan time)
        {
            TimeSpan closestTime = new TimeSpan();

            foreach (var item in timeTable)
            {
                if (item.StationName == stationName)
                {
                    closestTime = item.DepartureTimes.Where(t => t > time).OrderBy(t => Math.Abs((t - time).Ticks)).First();
                    return closestTime;
                }
            }
            return closestTime;
        }

        private static TimeSpan CalculateWalkingTime(Bus bus, double distance)
        {
            double walkSpeed = 1.4;
            TimeSpan walkTime = TimeSpan.FromSeconds(distance / walkSpeed);

            return walkTime;
        }

        private static TimeSpan CalculateBusrideTime(List<BusStation> timeTable, string endStation, TimeSpan nextBus)
        {
            TimeSpan timeWhenBusArrives = new TimeSpan();

            foreach (var item in timeTable)
            {
                if (item.StationName == endStation)
                {
                    timeWhenBusArrives = item.DepartureTimes.Where(t => t > nextBus).OrderBy(t => Math.Abs((t - nextBus).Ticks)).First();
                    //Lägga till att tiden som det tar att gå måste vara mer än nextBus, alltså t + interval(walkTime) > nextBus
                }
            }

            var resultBusRideTime = timeWhenBusArrives - nextBus;
            return resultBusRideTime;
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(TimeSpan nextBus)
        {
            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = nextBus - time.TimeOfDay; //Genom att använda time.TimeOfDay så kan time omvandlas till TimeSpan, går inte att subtrahera DateTime från TimeSpan
            return timeUntilBusDeparture;
        }

        private static TimeSpan CalculateTimeUntilYouNeedToGo(TimeSpan nextBus, TimeSpan walkingTimeToBus)
        {
            DateTime time = DateTime.Now;

            //Validering: Vad händer om det tar längre tid att gå till bussen än vadd det är kvar tills bussen går?
            var timeUntilYouNeedToStartWalking = nextBus.Subtract(time.TimeOfDay).Subtract(walkingTimeToBus);
            return timeUntilYouNeedToStartWalking;

        }

        private static void CountDown(TimeSpan timeUntilYouNeedToGo)
        {
            var oneSecond = new TimeSpan(0, 0, 01);
            var timeZero = new TimeSpan(0, 0, 0);
                var hurry = new TimeSpan(0,5,0);
                if (timeUntilYouNeedToGo < hurry)
                {
                    WriteRed("Nu har du bråttom!");
                }
                else
                    WriteGreen("Ta det lugnt, du har gott om tid");

            Console.Write("Minuter kvar tills du måste gå till bussen: ");

            while (true)
            {
                timeUntilYouNeedToGo = timeUntilYouNeedToGo.Subtract(oneSecond);
                Console.CursorLeft = 45;
                Console.Write($"{timeUntilYouNeedToGo.Hours} : {timeUntilYouNeedToGo.Minutes} : {timeUntilYouNeedToGo.Seconds}");

                if (timeUntilYouNeedToGo <= timeZero)
                {
                    WriteRed("Tiden är ute du misssa bussen tyvärr :( ");
                    break;
                }
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
            Console.WriteLine("YoshiHelper".PadLeft(30));
            Console.WriteLine("---------------------------------------------------------");
            Console.ResetColor();

        }




    }
}
