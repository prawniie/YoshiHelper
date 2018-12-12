using System;
using System.Collections.Generic;
using System.Text;

namespace YoshiHelper
{
    class Bus
    {
        public string startStation { get; set; }
        public string endStation { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
    }
}
