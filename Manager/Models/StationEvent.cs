using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Models
{
    public class StationEvent
    {
        public DateTime Time { get; set; }
        public Station Station { get; set; }
        public Airplane Airplane { get; set; }
        public StationEventType EventType { get; set; }
    }
}
