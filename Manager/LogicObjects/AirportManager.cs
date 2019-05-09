using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class AirportManager : IAirportManager
    {
        public AirportManager(IRouteManager routeManager, IAirportStateLoader loader)
        {
            RouteManager = routeManager;
            AirportState = loader.Load();
            StartingStations = loader.GetStartingPoints(AirportState);
        }
        Dictionary<FlightActionType, IStationService> StartingStations;
        public IRouteManager RouteManager { get; set; }

        public AirportState AirportState { get; set; }

        public void PushAirplane(Airplane airplane)
        {
            var startingStation = (StartStationService)StartingStations[airplane.ActionType];
            startingStation.MoveIn(airplane);
        }
    }
}
