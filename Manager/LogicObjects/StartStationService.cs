using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.LogicObjects
{
    class StartStationService : IStationService
    {
        public StartStationService(IRouteManager routeManager)
        {
            _routeManager = routeManager;
        }
        public Queue<Airplane> Queue { get; set; }
        IRouteManager _routeManager;
        public Station Station { get; set; }

        public bool waitingInQueue = false;

        public void MoveIn(Airplane airplane)
        {
            Queue.Enqueue(airplane);

            if (waitingInQueue == false)
            {
                _routeManager.Subscribe(this);
                waitingInQueue = true;
            }

        }

        public void MoveOut(IStationService stationServ)
        {
            if (Queue.Any())
            {
                var airplaneToMove = Queue.Dequeue();
                stationServ.MoveIn(airplaneToMove);

                if (Queue.Any())
                {
                    _routeManager.Subscribe(this);
                    waitingInQueue = true;
                }
                else
                {
                    waitingInQueue = false;
                }

            }

            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(stationServ));
        }
    }
}
