using Common.Models;
using Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    class StationServiceBuilder : IStationServiceBuilder
    {
        public StationServiceBuilder(IRouteManager routeManager, ITimer timer)
        {
            _stationService = new StationService(routeManager, timer);
            _stationService.NextStationsServices = new Dictionary<FlightActionType, List<IStationService>>();
        }
        private IStationService _stationService;

        public IStationServiceBuilder AddNextStationService(FlightActionType actionType, IStationService stationService)
        {
            if (!_stationService.NextStationsServices.ContainsKey(actionType))
            {
                _stationService.NextStationsServices.Add(actionType, new List<IStationService>());
            }            
           _stationService.NextStationsServices[actionType].Add(stationService);
            return this;
        }

        public IStationServiceBuilder AddStation(Station station)
        {
            if (_stationService.Station == null)
            {
                _stationService.Station = station;
                return this;
            }
            else
            {
                throw new Exception("Station already exist");
            }
        }

        public IStationService GetStationService()
        {
            return _stationService;
        }

        public Station GetStation()
        {
            return _stationService.Station;
        }

        public bool IsStationMatchService(Station station)
        {
            return (_stationService.Station == station);
        }
    }
}
