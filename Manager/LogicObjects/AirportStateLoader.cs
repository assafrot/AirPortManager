using Common.Models;
using DAL.Interfaces;
using Extenstions;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.LogicObjects
{
    public class AirportStateLoader : IAirportStateLoader
    {
        public AirportStateLoader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        IUnitOfWork _unitOfWork { get; set; }

        public AirportState Load()
        {
            AirportState state = new AirportState();
            var stationsDB = _unitOfWork.Stations.GetAll();
            var stations = new List<Station>();
            foreach (var station in stationsDB)
            {
                var stationDto = station.ToDTO();
                stationDto.NextStations = _unitOfWork.Stations.GetLinkedStation(station.Id);
                stations.Add(station.ToDTO());
            }
            state.Stations = stations;

            var landings = _unitOfWork.Flights.GetAllFutureFlights(FlightActionType.Landing);
            var takeoffs = _unitOfWork.Flights.GetAllFutureFlights(FlightActionType.Takeoff);
            state.AirplanesInQueue[FlightActionType.Landing] = landings.ToList();
            state.AirplanesInQueue[FlightActionType.Takeoff] = takeoffs.ToList();
            return state;
        }
    }
}
