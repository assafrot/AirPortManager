using System.Collections.Generic;

namespace Common.Models
{
    public class Station
    {
        public int Id { get; set; }
        public Flight Flight { get; set; }
        public bool IsEmpty { get => Flight == null;}
        public bool EndPoint { get; set; }
        public bool StartPoint { get; set; }
        public Dictionary<FlightActionType, List<Station>> NextStations { get; set; }
    }
}
