using Manager.Interfaces;
using System.Collections.Generic;

namespace Manager.Models
{
    public class Station
    {
        public Flight Airplane { get; set; }
        public bool IsEmpty { get => Airplane == null;}
        public bool EndPoint { get; set; }
        public bool StartPoint { get; set; }
        public Dictionary<FlightActionType, List<IStationService>> NextStations { get; set; }
    }
}
