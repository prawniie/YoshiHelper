﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace YoshiHelper
{
    //Fråga Oscar hur göra för att kunna komma tillbaka till menyn när man fått svar på när bussen går!!

    public class Program
    {
        static Bus bus = new Bus();
        static List<BusStation> timeTable = new List<BusStation>();

        static void Main(string[] args)
        {

            WelcomeUser();
            timeTable = ReadTimetable();
            AskUserForDefaultSettings();
            Console.Clear();

            DisplayMenu();
        }

        private static void WelcomeUser()
        {
            Header();
            Console.WriteLine("\nInnan Yoshi kan hjälpa till behöver vi veta lite information om dig:\n");
        }

        private static void AskUserForDefaultSettings()
        {
            while (true)
            {
                try
                {
                    Console.Write("Vilken busshållplats går du till efter jobbet? ");
                    string input = Console.ReadLine();
                    if (!Regex.IsMatch(input, @"^(Åkareplatsen|Svingeln|Munkebäcksmotet|Stenkullen|Gråbo)$", RegexOptions.IgnoreCase))
                    {
                        WriteRed("Kan inte hitta hållplatsen, vänligen skriv in igen");
                    }
                    else
                    {
                        bus.StartStation = input;
                        break;
                    }
                }
                catch (Exception)
                {
                    WriteRed("Något gick fel..");
                }
            }

            while (true)
            {
                try
                {
                    Console.Write("Hur långt är det till busshållplatsen från jobbet (i meter)? ");
                    bus.DistanceToStartStation = double.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    WriteRed("Du ska knappa in en siffra!!!!!!!!!!!!!!!");
                }
            }

            while (true)
            {
                try
                {
                    Console.Write("Vilken busshållplats hoppar du av på? ");
                    string input = Console.ReadLine();
                    if (!Regex.IsMatch(input, @"^(Åkareplatsen|Svingeln|Munkebäcksmotet|Stenkullen|Gråbo)$", RegexOptions.IgnoreCase))
                    {
                        WriteRed("Kan inte hitta hållplatsen, vänligen skriv in igen");
                    }
                    else
                    {
                        bus.EndStation = input;
                        break;
                    }
                }
                catch (Exception)
                {
                    WriteRed("Hållplatsen finns inte");
                }
            }

            while (true)
            {
                try
                {
                    Console.Write("Hur långt är det från denna hållplats hem till dig (i meter)? ");
                    bus.DistanceToEndStation = double.Parse(Console.ReadLine());
                    break;

                }
                catch (Exception)
                {
                    WriteRed("Du ska knappa in en siffra!!!!!!!!!!!!!!!");
                }
            }
            WriteGreen("Dina ändringar är sparade!"); //Detta skrivs inte heller ut
            DisplayMenu();
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            WriteGreen("MENY");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] När ska jag gå för att hinna med nästa buss? ");
            Console.WriteLine("[2] Inställningar");
            Console.WriteLine("[3] Exit");
            Console.ResetColor();

            string choice = "";

            do
            {
                Console.Write("Välj 1,2 eller 3: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        WhenShouldIGo(timeTable);
                        break;

                    case "2":
                        AskUserForDefaultSettings();
                        break;
                    case "3":
                        break;

                    default:
                        {
                            Console.WriteLine("Ogiltigt val. Skriv in 1, 2 eller 3"); //Varför skriver den inte ut felmeddelandet? Oklart..
                            DisplayMenu();
                            break;
                        }
                }
            } while (choice != "3");
        }

        private static void WhenShouldIGo(List<BusStation> timeTable)
        {
            TimeSpan targetTime = DateTime.Now.TimeOfDay;

            try
            {
                TimeSpan walkingTimeToBus = CalculateWalkingTime(bus, bus.DistanceToStartStation);
                TimeSpan walkingTimeToWork = CalculateWalkingTime(bus, bus.DistanceToEndStation);
                TimeSpan nextBus = FindNextBus(timeTable, bus.StartStation, targetTime, walkingTimeToBus);

                TimeSpan timeUntilBusDeparture = CalculateTimeUntilBusDeparture(nextBus);
                TimeSpan timeUntilYouNeedToGo = CalculateTimeUntilYouNeedToGo(nextBus, walkingTimeToBus);

                CalculateBusrideTime(timeTable, bus.EndStation, nextBus);

                CountDown(timeUntilYouNeedToGo);

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex);
                DisplayMenu(); //Om det blir knas så fastnar man inte i loopen utan får komma till menyn istället 
                               //Men inget felmeddelande skrivs ut!
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DisplayMenu();

            }
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

        public static TimeSpan FindNextBus(List<BusStation> timeTable, string stationName, TimeSpan time, TimeSpan walkingTimeToBus)
        {
            TimeSpan closestTime = new TimeSpan();

            foreach (var item in timeTable)
            {
                if (item.StationName == stationName)
                {
                    closestTime = item.DepartureTimes.Where(t => t > time.Add(walkingTimeToBus)).OrderBy(t => Math.Abs((t - time).Ticks)).First();
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
                }
            }

            var resultBusRideTime = timeWhenBusArrives - nextBus;
            return resultBusRideTime;
        }

        private static TimeSpan CalculateTimeUntilBusDeparture(TimeSpan nextBus)
        {
            DateTime time = DateTime.Now;

            var timeUntilBusDeparture = nextBus - time.TimeOfDay;
            return timeUntilBusDeparture;
        }

        private static TimeSpan CalculateTimeUntilYouNeedToGo(TimeSpan nextBus, TimeSpan walkingTimeToBus)
        {
            TimeSpan timeUntilYouNeedToStartWalking = new TimeSpan(0, 0, 0);
            DateTime time = DateTime.Now;
            try
            {
                timeUntilYouNeedToStartWalking = nextBus.Subtract(time.TimeOfDay).Subtract(walkingTimeToBus);
                if (timeUntilYouNeedToStartWalking < new TimeSpan(0, 0, 0))
                {
                    WriteRed("Något gick fel, det tar för lång tid att gå så du kommer missa bussen.."); //Detta skrivs inte ut..
                    throw new ArgumentException("Fast i loopen..");
                }
                else
                {
                    return timeUntilYouNeedToStartWalking;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel");
            }

            return timeUntilYouNeedToStartWalking;
        }

        private static void CountDown(TimeSpan timeUntilYouNeedToGo)
        {
            var oneSecond = new TimeSpan(0, 0, 01);
            var timeZero = new TimeSpan(0, 0, 0);
            var hurry = new TimeSpan(0, 5, 0);
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
                Console.Write($"{timeUntilYouNeedToGo.Hours.ToString().PadLeft(2)} : {timeUntilYouNeedToGo.Minutes.ToString().PadLeft(2)} : {timeUntilYouNeedToGo.Seconds.ToString().PadLeft(2)}");

                if (timeUntilYouNeedToGo <= timeZero)
                {
                    WriteRed("Tiden är ute, nu kommer du misssa bussen tyvärr :( ");
                    break;
                }
                System.Threading.Thread.Sleep(1000);

                //Console.WriteLine("\nTryck enter för att komma tillbaka till menyn");
                //string input = Console.ReadLine();
                //if (string.IsNullOrWhiteSpace(input))
                //{
                //    DisplayMenu();
                //    break;
                //}
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
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();

            Console.WriteLine(@"$$\     $$\                   $$\       $$\       $$\   $$\           $$\");
            Console.WriteLine(@"\$$\   $$  |                  $$ |      \__|      $$ |  $$ |          $$ |");
            Console.WriteLine(@" \$$\ $$  /$$$$$$\   $$$$$$$\ $$$$$$$\  $$\       $$ |  $$ | $$$$$$\  $$ | $$$$$$\   $$$$$$\   $$$$$$\  ");
            Console.WriteLine(@"  \$$$$  /$$  __$$\ $$  _____|$$  __$$\ $$ |      $$$$$$$$ |$$  __$$\ $$ |$$  __$$\ $$  __$$\ $$  __$$\ ");
            Console.WriteLine(@"   \$$  / $$ /  $$ |\$$$$$$\  $$ |  $$ |$$ |      $$  __$$ |$$$$$$$$ |$$ |$$ /  $$ |$$$$$$$$ |$$ |  \__|");
            Console.WriteLine(@"    $$ |  $$ |  $$ | \____$$\ $$ |  $$ |$$ |      $$ |  $$ |$$   ____|$$ |$$ |  $$ |$$   ____|$$ |   ");
            Console.WriteLine(@"    $$ |  \$$$$$$  |$$$$$$$  |$$ |  $$ |$$ |      $$ |  $$ |\$$$$$$$\ $$ |$$$$$$$  |\$$$$$$$\ $$ |   ");
            Console.WriteLine(@"    \__|   \______/ \_______/ \__|  \__|\__|      \__|  \__| \_______|\__|$$  ____/  \_______|\__|    ");
            Console.WriteLine(@"                                                                          $$ |                         ");
            Console.WriteLine(@"                                                                          $$ |                        ");
            Console.WriteLine(@"                                                                          \__|                        ");
            Console.ResetColor();


        }
    }
}
