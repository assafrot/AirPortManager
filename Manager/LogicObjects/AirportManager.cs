using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class AirportManager : IAirportManager
    {
        public AirportManager(IRouteManager routeManager, ITimer timer ,IAirportStateLoader loader)
        {
            _timer = timer;
            RouteManager = routeManager;          
            AirportState = loader.Load();            
            StartingStations = new Dictionary<FlightActionType, IStationService>();
            Init();
        }
        Dictionary<FlightActionType, IStationService> StartingStations;
        public IRouteManager RouteManager { get; set; }
        private ITimer _timer;
        private List<IStationServiceBuilder> _buildersList;
        public AirportState AirportState { get; set; }

        public void PushAirplane(Flight airplane)
        {
            var startingStation = (StartStationService)StartingStations[airplane.ActionType];
            startingStation.MoveIn(airplane);
        }

        private void Init()
        {
            _buildersList = new List<IStationServiceBuilder>();
            foreach (Station station in AirportState.Stations)
            {
                var builder = new StationServiceBuilder(RouteManager, _timer);
                _buildersList.Add(builder.AddStation(station));
                if (station.StartPoint)
                {
                    var direction = GetStartingPointDirection(station);
                    StartingStations.Add(direction, builder.GetStationService());
                }
            }
            foreach (var builder in _buildersList)
            {
                var station = builder.GetStation();
                LinkNextService(builder, station, FlightActionType.Landing);
                LinkNextService(builder, station, FlightActionType.Takeoff);                               
            }

        }

        private FlightActionType GetStartingPointDirection(Station station)
        {
            if (station.NextStations.ContainsKey(FlightActionType.Landing))
            {
                return FlightActionType.Landing;
             }
            else
                return FlightActionType.Takeoff;
        }

        private void LinkNextService(IStationServiceBuilder builder, Station station, FlightActionType flightActionType)
        {
            foreach (var nextStation in station.NextStations[flightActionType])
            {
                var nextService = GetNextService(nextStation);
                if (nextService != null)
                {
                builder.AddNextStationService(FlightActionType.Landing, nextService);
                }
            }
        }

        private IStationService GetNextService(Station nextStation)
        {
            IStationService res = null;
            foreach (var builder in _buildersList)
            {
                if (builder.IsStationMatchService(builder.GetStation()))
                {
                    res = builder.GetStationService();
                    break;
                }
            }
            return res;
        }

    }
}
