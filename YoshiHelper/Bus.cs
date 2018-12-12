using System;
using System.Collections.Generic;
using System.Text;

namespace YoshiHelper
{
    class Bus
    {
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double DistanceToStartStation { get; set; }
        public double DistanceToEndStation { get; set; }
    }
}
