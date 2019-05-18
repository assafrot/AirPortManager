using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.LogicObjects
{
    public class StationService : IStationService
    {

        public StationService(IRouteManager routeManager, ITimer timer)
        {
            _timer = timer;
            _routeManager = routeManager;
        }

        IRouteManager _routeManager;
        ITimer _timer;

        public Station Station { get; set; }
        public Dictionary<FlightActionType, List<IStationService>> NextStationsServices { get; set; }

        public async void MoveIn(Flight airplane)
        {
            Station.Flight = airplane;
            await _timer.Wait(2000);
            _routeManager.Subscribe(this);
        }

        public void MoveOut(IStationService stationServ)
        {
            var airplaneToMove = Station.Flight;
            Station.Flight = null;
            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(this));
            stationServ.MoveIn(airplaneToMove);
        }

    }
}
