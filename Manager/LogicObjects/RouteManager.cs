﻿using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Manager.LogicObjects
{
    public class RouteManager : IRouteManager
    {
        public RouteManager()
        {
            _stationQueue = new Dictionary<IStationService, QList<IStationService>>();
        }

        private Dictionary<IStationService, QList<IStationService>> _stationQueue;

        public event Action<StationEvent> OnAirplaneMoved;
        public event Action<Flight> OnAirplaneDequeue;

        public void NotifyStationEmptied(StationEmptiedEventArgs args)
        {

            if (_stationQueue.Keys.Contains(args.StationService) == false)
            {
                _stationQueue.Add(args.StationService, new QList<IStationService>());
            }

            var haveSubs = _stationQueue.TryGetValue(args.StationService, out var queue);
            bool any;

            lock (queue)
            {
                any = queue.Any();
            }

            if (haveSubs && any)
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
                Debug.WriteLine(station.Station.Flight);
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
                if(!_stationQueue.ContainsKey(stationToSub))
                {
                    _stationQueue.Add(stationToSub, new QList<IStationService>());
                }

                _stationQueue[stationToSub].Enqueue(stationServ);
            }
           
        }

        public void Unsubscribe(IStationService stationServ)
        {
            var stationService = stationServ;
            var flight = stationService.Station.Flight;
            stationService.NextStationsServices[flight.ActionType].ForEach(stationToUnsub =>
            {
                _stationQueue[stationToUnsub].Remove(stationServ);
            });
        }

    }
}
