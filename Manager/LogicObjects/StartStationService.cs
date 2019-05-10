using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.LogicObjects
{
    public class StartStationService : IStationService
    {
        public StartStationService(Station station, IRouteManager routeManager)
        {
            Station = station;
            _routeManager = routeManager;
        }

        public Queue<Flight> Queue { get; set; } = new Queue<Flight>();
        IRouteManager _routeManager;
        public Station Station { get; set; }
        public bool GotAirplanesInQueue { get => Queue.Any();}
        bool subToRouteManager = false;

        public void MoveIn(Flight airplane)
        {
            lock (Queue)
            {
                Queue.Enqueue(airplane);
                if (subToRouteManager == false)
                {
                    subToRouteManager = true;
                    Station.Airplane = airplane;
                    _routeManager.Subscribe(this);
                }
            }
        }


        public void MoveOut(IStationService stationServ)
        {
            if (GotAirplanesInQueue)
            {
                var airplaneToMove = Queue.Dequeue();
                stationServ.MoveIn(airplaneToMove);

                if (GotAirplanesInQueue)
                {
                    Station.Airplane = Queue.Peek();
                    _routeManager.Subscribe(this);
                } else
                {
                    subToRouteManager = false;
                }

            }

            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(stationServ));
        }
    }
}
