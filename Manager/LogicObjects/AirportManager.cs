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
        public AirportManager(IRouteManager routeManager, ITimer timer 
            ,IAirportStateLoader loader, IStationServicesBuilder stationServicesBuilder)
        {
            _stationServicesBuilder = stationServicesBuilder;
            _timer = timer;
            RouteManager = routeManager;          
            AirportState = loader.Load();                        
            Init();
        }
        Dictionary<FlightActionType, IStationService> StartingStations;
        public IRouteManager RouteManager { get; set; }
        private IStationServicesBuilder _stationServicesBuilder;
        private ITimer _timer;
        public AirportState AirportState { get; set; }

        public void PushFlight(Flight airplane)
        {
            var startingStation = StartingStations[airplane.ActionType];
            startingStation.MoveIn(airplane);
        }

        private void Init()
        {
            _stationServicesBuilder.BuildServices(AirportState.Stations);
            StartingStations = _stationServicesBuilder.StartingStations;
        }


    }
}
