using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class StationsLinks
    {
        public int OriginStationId { get; set; }
        public int DestinationStationId { get; set; }
        public FlightActionType ActionType { get; set; }
        public StationDB OriginStation { get; set; }
        public StationDB DestinationStation { get; set; }
    }
}
