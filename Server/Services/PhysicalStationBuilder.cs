using DAL.Interfaces;
using DAL.Models;
using Manager.Interfaces;
using Newtonsoft.Json;
using Server.Interfaces;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class PhysicalStationBuilder : IPhysicalStationBuilder
    {
        public PhysicalStationBuilder(IAirportManager airportManager)
        {
            _airportManager = airportManager;
            _physicalStations = new List<PhysicalStation>();
        }

        IAirportManager _airportManager;
        List<PhysicalStation> _physicalStations;
        public List<PhysicalStation> GetPhysicalStations()
        {
            var stations = _airportManager.AirportState.Stations;

            int yDelta = 0;
            for (int i = 0; i < stations.Count; i++)
            {
                if (i % 3 == 0) { yDelta++; }
                var physicalStation = new PhysicalStation()
                {
                    Id = stations[i].Id,
                    Flight = stations[i].Flight,
                    EndPoint = stations[i].EndPoint,
                    StartPoint = stations[i].StartPoint,
                    NextStations = stations[i].NextStations,
                    Height = 50,
                    Width = 50,
                    X = ((i%3 + 1) * 80),
                    Y = (80) * (yDelta)
                };
                _physicalStations.Add(physicalStation);
            }
            return _physicalStations;
        }
    }
}
