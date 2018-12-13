using System;
using System.Collections.Generic;
using System.Text;

namespace YoshiHelper
{
    //public enum StationName
    //{
    //    Åkareplatsen, Svingeln, Munkebäcksmotet
    //}

    public class BusStation
    {
        public string StationName { get; set; }
        public List<TimeSpan> DepartureTimes { get; set; }
    }
}
