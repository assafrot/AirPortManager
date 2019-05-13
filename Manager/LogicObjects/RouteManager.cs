using Common.Models;
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

        public event Action<StationEvent> OnAirplaneMoved;
        public event Action<Flight> OnAirplaneDequeue;

        public void NotifyStationEmptied(StationEmptiedEventArgs args)
        {
            var haveSubs = _stationQueue.TryGetValue(args.StationService, out var queue);
            
            if (haveSubs && queue.Any())
            {
                var stationServiceToNotify = queue.Dequeue();
                if (stationServiceToNotify is StartStationService)
                {
                    var flight = stationServiceToNotify.Station.Flight;
                    flight.InQueue = false;
                    OnAirplaneDequeue?.Invoke(flight);
                }
                Unsubscribe(stationServiceToNotify);
                stationServiceToNotify.MoveOut(args.StationService);

          

                OnAirplaneMoved?.Invoke(new StationEvent()
                {
                    Flight = args.StationService.Station.Flight,
                    EventType = StationEventType.Entered,
                    Station = args.StationService.Station,
                    Time = DateTime.Now
                });

                OnAirplaneMoved?.Invoke(new StationEvent()
                {
                    Flight = stationServiceToNotify.Station.Flight,
                    EventType = StationEventType.Existed,
                    Station = stationServiceToNotify.Station,
                    Time = DateTime.Now
                });



            }
        }

        public void Subscribe(IStationService stationServ)
        {
            var station = stationServ;
            lock (stationServ)
            {
                foreach (var nextStation in station.NextStationsServices[station.Station.Flight.ActionType])
                {
                    if (nextStation.Station.IsEmpty)
                    {
                        stationServ.MoveOut(nextStation);
                        OnAirplaneMoved?.Invoke(new StationEvent()
                        {
                            Flight = nextStation.Station.Flight,
                            EventType = StationEventType.Entered,
                            Station = nextStation.Station,
                            Time = DateTime.Now
                        });

                        OnAirplaneMoved?.Invoke(new StationEvent()
                        {
                            Flight = stationServ.Station.Flight,
                            EventType = StationEventType.Existed,
                            Station = stationServ.Station,
                            Time = DateTime.Now
                        });

                        return;
                    }
                }
            }


            foreach (var stationToSub in station.NextStationsServices[station.Station.Flight.ActionType])
            {
                _stationQueue[stationToSub].Enqueue(stationServ);
            }
           
        }

        public void Unsubscribe(IStationService stationServ)
        {
            var stationService = stationServ;

            stationService.NextStationsServices[stationService.Station.Flight.ActionType].ForEach(stationToUnsub =>
            {
                _stationQueue[stationToUnsub].Remove(stationServ);
            });
        }

    }
}
