using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public FlightActionType ActionType { get; set; }
        public DateTime RequestedTime { get; set; }
        public bool InQueue { get; set; }
    }
}
