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

            TimeSpan walkingTimeToBus = CalculateWalkingTimeToBus(bus);
            TimeSpan walkingTimeToWork = CalculateWalkingTimeToWork(bus);

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
                if (item.StationName == stationName)
                {
                    closestTime = item.DepartureTimes.Where(t => t > time).OrderBy(t => Math.Abs((t - time).Ticks)).First();
                    return closestTime;
                }
            }
            return closestTime;
        }

        private static TimeSpan CalculateWalkingTimeToBus(Bus bus)
        {
            double walkSpeed = 1.4;

            var walktoStartStationSeconds = bus.DistanceToStartStation / walkSpeed;
            Console.WriteLine($"Det tar {string.Join(",",walktoStartStationSeconds/60):#.#} minuter att gå till bussen hemifrån");

            TimeSpan interval = TimeSpan.FromSeconds(walktoStartStationSeconds); //konverterar sekunderna det tar att gå till timeSpan från double

            return interval;
        }

        private static TimeSpan CalculateWalkingTimeToWork(Bus bus)
        {
            double walkSpeed = 1.4;

            var walktoEndStationSeconds = bus.DistanceToEndStation / walkSpeed;
            Console.WriteLine($"Det tar {string.Join(",", walktoEndStationSeconds/60):#.#} minuter att gå till bussen från jobbet");

            TimeSpan interval = TimeSpan.FromSeconds(walktoEndStationSeconds); //konverterar sekunderna det tar att gå till timeSpan från double

            return interval;
        }

        private static TimeSpan CalculateBusrideTime(List<DepartureTime> timeTable, string endStation, TimeSpan nextBus)
        {
            TimeSpan timeWhenBusArrives = new TimeSpan();

            foreach (var item in timeTable)
            {
                if (item.StationName == endStation)
                {
                    timeWhenBusArrives = item.DepartureTimes.Where(t => t > nextBus).OrderBy(t => Math.Abs((t - nextBus).Ticks)).First();
                }
            }

            var resultBusRideTime = timeWhenBusArrives - nextBus;
            return resultBusRideTime;
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(TimeSpan nextBus)
        {
            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = nextBus - time.TimeOfDay; //Genom att använda time.TimeOfDay så kan time omvandlas till TimeSpan, går inte att subtrahera DateTime från TimeSpan
            Console.WriteLine($"\nDet är {timeUntilBusDeparture:mm} minuter kvar tills bussen går");

            return timeUntilBusDeparture;
        }

        private static TimeSpan CalculateTimeUntilYouNeedToGo(TimeSpan nextBus, TimeSpan walkingTimeToBus)
        {
            DateTime time = DateTime.Now;

            var timeUntilYouNeedToStartWalking = nextBus.Subtract(time.TimeOfDay).Subtract(walkingTimeToBus);
            Console.WriteLine($"\nDet är {timeUntilYouNeedToStartWalking} minuter kvar tills du måste gå till bussen!");
            return timeUntilYouNeedToStartWalking;

        }

        private static void CountDown(TimeSpan timeUntilYouNeedToGo)
        {
            // 123456s  / 3600 = timmar

            // 80sekunder / 60 =1min
            // 80 % 60 =       20sek

            //var onesecond = new TimeSpan(0, 0, 1);

            //var t = new TimeSpan(1, 2, 0);
            //t = t.Subtract(onesecond);
            //Console.WriteLine(t.ToString());


            //var t = timeUntilYouNeedToGo.Minutes;
            //int countdown = timeUntilYouNeedToGo.Minutes;

            //int countdown = timeUntilYouNeedToGo

            //for (double countdown = timeUntilYouNeedToGo.TotalSeconds; countdown >= 0; countdown--) // a = 10 symboliserar 10 min kvar tills bussen går
            //{
            //    timeLeft = timeUntilYouNeedToGo.Subtract(oneSecond);
            //    Console.CursorLeft = 45;
            //    Console.Write(timeLeft);
            //    System.Threading.Thread.Sleep(1000);
            //}

            Console.Write("Minuter kvar tills du måste gå till bussen: ");

            var timeLeft = new TimeSpan();
            var oneSecond = new TimeSpan(0, 0, 1);
            var timeZero = new TimeSpan(0, 0, 0);

           // double countdown = timeUntilYouNeedToGo.TotalSeconds;


            while (true)
            {

                timeUntilYouNeedToGo = timeUntilYouNeedToGo.Subtract(oneSecond);
                Console.CursorLeft = 45;
                Console.Write($"{timeUntilYouNeedToGo.Hours} : {timeUntilYouNeedToGo.Minutes} : {timeUntilYouNeedToGo.Seconds}");

                if (timeUntilYouNeedToGo == timeZero)
                {
                    break;
                }

                System.Threading.Thread.Sleep(1000);
                //countdown--;

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
