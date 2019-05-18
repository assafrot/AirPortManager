using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class StationEventDB
    {
        public DateTime Time { get; set; }
        public StationDB Station { get; set; }
        public Flight Flight { get; set; }
        public StationEventType EventType { get; set; }
        public int Id { get; set; }
        public int StationId { get; set; }
        public int FlightId { get; set; }
    }
}
