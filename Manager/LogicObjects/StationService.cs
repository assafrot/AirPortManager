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

        public StationService(Station station, IRouteManager routeManager)
        {
            _routeManager = routeManager;
            Station = station;
        }

        IRouteManager _routeManager;

        public Station Station { get; set; }

        public async void MoveIn(Flight airplane)
        {
            Station.Airplane = airplane;
            await Timer();
            _routeManager.Subscribe(this);
        }

        Task Timer()
        {
            return Task.Run(() =>
            {
                Random rnd = new Random();
                Thread.Sleep(rnd.Next(3)+1 * 200);
            });
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
