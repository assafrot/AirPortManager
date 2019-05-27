using Common.Models;
using Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class StationServicesBuilder : IStationServicesBuilder
    {
        public StationServicesBuilder(IRouteManager routeManager, ITimer timer)
        {
            _routeManager = routeManager;
            _timer = timer;
            StartingStations = new Dictionary<FlightActionType, IStationService>();

        }


        IRouteManager _routeManager;
        ITimer _timer;
        List<IStationService> _stationServices;
        public Dictionary<FlightActionType, IStationService> StartingStations { get; set; }

        public List<IStationService> BuildServices(List<Station> stations)
        {
            _stationServices = new List<IStationService>();
            //init stations
            foreach (var station in stations)
            {
                IStationService stationService;
                if (station.StartPoint)
                {
                    stationService = new StartStationService(_routeManager, _timer);
                }
                else if (station.EndPoint)
                 {
                    stationService = new EndStationService(_routeManager, _timer);

                }
                else
                {
                    stationService = new StationService(_routeManager, _timer);
                }
                stationService.Station = station;
                stationService.NextStationsServices = new Dictionary<FlightActionType, List<IStationService>>();
                _stationServices.Add(stationService);
            }
            //conect to next services
            foreach (var stationService in _stationServices)
            {
                var station = stationService.Station;
                LinkNextService(stationService, station, FlightActionType.Landing);
                LinkNextService(stationService, station, FlightActionType.Takeoff);
                if (stationService is StartStationService)
                {
                    var direction = GetStartingPointDirection(station);
                     StartingStations.Add(direction, stationService);
                }

            }

            return _stationServices;
        }

        private void AddNextStationService(IStationService stationService, 
            FlightActionType actionType, IStationService nextStationService)
        {
            if (!stationService.NextStationsServices.ContainsKey(actionType))
            {
                stationService.NextStationsServices.Add(actionType, new List<IStationService>());
            }
            stationService.NextStationsServices[actionType].Add(nextStationService);
        }

        private FlightActionType GetStartingPointDirection(Station station)
        {
            if (station.NextStations.ContainsKey(FlightActionType.Landing)
                && station.NextStations[FlightActionType.Landing].Count>0)
            {
                return FlightActionType.Landing;
            }
            else
            {
                return FlightActionType.Takeoff;
            }
        }

        private IStationService GetStationService(Station nextStation)
        {

            foreach (var stationService in _stationServices)
            {
               if (stationService.Station.Id == nextStation.Id)
                {
                    return stationService;
                }
            }

            return null;
        }

        private void LinkNextService(IStationService stationService, Station station, FlightActionType flightActionType)
        {
            foreach (var nextStation in station.NextStations[flightActionType])
            {
                var nextService = GetStationService(nextStation);
                if (nextService != null)
                {
                    AddNextStationService(stationService, FlightActionType.Landing, nextService);
                }
            }
        }
    }
}
