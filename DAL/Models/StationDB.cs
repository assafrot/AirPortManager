using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class StationDB
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public bool IsEmpty { get => Flight == null; }
        public bool EndPoint { get; set; }
        public bool StartPoint { get; set; }       
    }
}
