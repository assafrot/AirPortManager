using Common.Models;
using DAL.Interfaces;
using Manager.Interfaces;
using Manager.Models;

using Server.Extenstions;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AirportStateArchiver : IAirportStateArchiver
    {
        public IUnitOfWork UnitOfWork { get; set; }
        IAirportManager _airportManager;
        public AirportStateArchiver(IUnitOfWork unitOfWork, IAirportManager airportManager)
        {
            UnitOfWork = unitOfWork;
            _airportManager = airportManager;
        }
        public void StartArchiving()
        {
            _airportManager.RouteManager.OnAirplaneMoved += OnAirplaneMoved;
            _airportManager.RouteManager.OnAirplaneDequeue += FlightsStatusUpdate;
        }

        private void OnAirplaneMoved (StationEvent stationEvent)
        {
            AirportLogger(stationEvent);
            StationStateUpdate(stationEvent.Station);
        }

        private void AirportLogger(StationEvent stationEvent)
        {
            UnitOfWork.StationHistory.Add(stationEvent.ToDB());
            UnitOfWork.Commit();
        }

        private void StationStateUpdate(Station station)
        {
            var stationToUpdate = UnitOfWork.Stations.Get(station.Id);
            stationToUpdate.Flight = station.Flight;
            UnitOfWork.Commit();
        }

        private void FlightsStatusUpdate(Flight flight)
        {
            var flightToUpdate = UnitOfWork.Flights.Get(flight.Id);
            flightToUpdate.InQueue = flight.InQueue;
            UnitOfWork.Commit();
        }
    }
}
