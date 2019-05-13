using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class StationEventDB : StationEvent
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public int FlightId { get; set; }
    }
}
