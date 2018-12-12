﻿using System;
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
            
        }

        private static void CalculateWalkingTime(List<Bus> busInfo)
        {
            double walkSpeed = 1.4;
            var walktoStartStationSeconds = busInfo.Select(x => x.DistanceToStartStation / walkSpeed);
            double walktoStartStationMinutes = Convert.ToDouble(walktoStartStationSeconds) / 60;

            Console.WriteLine($"It takes {walktoStartStationMinutes:0.##} minutes to walk to the start station from home");

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
            bus.DistanceToStartStation = int.Parse(Console.ReadLine());

            Console.Write("Enter end bus station: ");
            bus.EndStation = Console.ReadLine();

            Console.Write("Enter distance from end bus station to work (in m): ");
            bus.DistanceToEndStation = int.Parse(Console.ReadLine());

            busInfo.Add(bus);
            return busInfo;

        }

        private static void ReadTimetable()
        {
            string[] timeTable = File.ReadAllLines(@"C:\Project\YoshiHelper\YoshiHelper\YoshiHelper\Gråbosnabben.txt");
        }




    }
}
