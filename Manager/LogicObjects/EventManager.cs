using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.LogicObjects
{
   public class EventManager : IEventManager
    {
        private static EventManager _instance;
        private Dictionary<IStationService, QList<IStationService>> _stationQueue;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventManager();
                return _instance;
            }
        }

        public void Emit(StationEmptiedEventArgs args)
        {
            var haveSubs = _stationQueue.TryGetValue(args.StationService, out var queue);

            if (haveSubs && queue.Any())
            {
                var stationToNotify = queue.Dequeue();
                Unsubscribe(stationToNotify);
                stationToNotify.MoveOut(args.StationService);
            }
        }

        public void Subscribe(IStationService stationServ)
        {
            var station = stationServ.Station;
            station.NextStations[station.Airplane.ActionType].ForEach(stationToSub =>
            {
                _stationQueue[stationToSub].Enqueue(stationServ);
            });
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
