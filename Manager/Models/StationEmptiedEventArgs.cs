using Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Models
{
    public class StationEmptiedEventArgs
    {
        public StationEmptiedEventArgs(IStationService stationServ)
        {
            StationService = stationServ;
        }
        public IStationService StationService { get; set; }
    }
}
