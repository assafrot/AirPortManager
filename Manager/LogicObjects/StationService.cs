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

        public StationService(Station station, IRouteManager routeManager, ITimer timer)
        {
            _timer = timer;
            _routeManager = routeManager;
            Station = station;
        }

        IRouteManager _routeManager;
        ITimer _timer;

        public Station Station { get; set; }

        public async void MoveIn(Flight airplane)
        {
            Station.Airplane = airplane;
            await _timer.Wait(2000);
            _routeManager.Subscribe(this);
        }

        public void MoveOut(IStationService stationServ)
        {
            var airplaneToMove = Station.Airplane;
            Station.Airplane = null;
            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(this));
            stationServ.MoveIn(airplaneToMove);
        }

    }
}
