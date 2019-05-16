using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class AirportState
    {
        public List<Station> Stations { get; set; }
        public Dictionary<FlightActionType, List<Flight>> AirplanesInQueue { get; set; }
    }
}
