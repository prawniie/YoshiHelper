using System;
using System.Collections.Generic;
using System.Text;

namespace YoshiHelper
{
    public class DepartureTime
    {
        public string StationName { get; set; }
        public List<TimeSpan> DepartureTimes { get; set; }
    }
}
