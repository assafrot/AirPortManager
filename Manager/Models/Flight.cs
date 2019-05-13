using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Models
{
    public class Flight
    {
        public string Model { get; set; }
        public FlightActionType ActionType { get; set; }
        public DateTime PlannedTime { get; set; }
        public bool InQueue { get; set; }
    }
}
