using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.LogicObjects
{
    public class RouteManager : IRouteManager
    {
        public RouteManager()
        {
            _stationQueue = new Dictionary<IStationService, QList<IStationService>>();
        }
        private static RouteManager _instance;
        private Dictionary<IStationService, QList<IStationService>> _stationQueue;

        public static RouteManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RouteManager();
                return _instance;
            }
        }

        public event Action<Station> OnAirplaneMovedIn;
        public event Action<Station> OnAirplaneMovedOut;

        public void NotifyStationEmptied(StationEmptiedEventArgs args)
        {
            var haveSubs = _stationQueue.TryGetValue(args.StationService, out var queue);

            if (haveSubs && queue.Any())
            {
                var stationServiceToNotify = queue.Dequeue();
                Unsubscribe(stationServiceToNotify);
                stationServiceToNotify.MoveOut(args.StationService);

                OnAirplaneMovedIn?.Invoke(args.StationService.Station);
                OnAirplaneMovedOut?.Invoke(stationServiceToNotify.Station);
            }
        }

        

        public void Subscribe(IStationService stationServ)
        {
            var station = stationServ.Station;
            lock (stationServ)
            {
                foreach (var stationToSub in station.NextStations[station.Airplane.ActionType])
                {
                    if (stationToSub.Station.IsEmpty)
                    {
                        stationServ.MoveOut(stationToSub);
                        return;
                    }
                }
            }


            foreach (var stationToSub in station.NextStations[station.Airplane.ActionType])
            {
                _stationQueue[stationToSub].Enqueue(stationServ);
            }
           
        }


        public void Unsubscribe(IStationService stationServ)
        {
            var station = stationServ.Station;

            station.NextStations[station.Airplane.ActionType].ForEach(stationToUnsub =>
            {
                _stationQueue[stationToUnsub].Remove(stationServ);
            });
        }









    }
}
