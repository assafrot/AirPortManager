using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Models
{
    public class AirportState
    {
        public List<Station> Stations { get; set; }
        public Dictionary<AirplaneActionType, List<Airplane>> AirplanesInQueue { get; set; }
    }
}
