using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class EndStationService : IStationService
    {

        public EndStationService(IRouteManager routeManager, ITimer timer)
        {
            _routeManager = routeManager;
        }

        public Station Station { get; set; }
        public Dictionary<FlightActionType, List<IStationService>> NextStationsServices { get; set; }

        IRouteManager _routeManager;

        public void MoveIn(Flight airplane)
        {
            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(this));
        }

        public void MoveOut(IStationService stationServ)
        {
            throw new Exception("Cant Move Out From End Point Station");
        }
    }
}
