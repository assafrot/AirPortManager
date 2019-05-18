using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class StationsLinks
    {
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public FlightActionType ActionType { get; set; }
    }
}
