using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class StationEvent
    {
        public DateTime Time { get; set; }
        public Station Station { get; set; }
        public Flight Flight { get; set; }
        public StationEventType EventType { get; set; }
    }
}
