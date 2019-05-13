using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IRouteManager
    {
        event Action<StationEvent> OnAirplaneMoved;
        void NotifyStationEmptied(StationEmptiedEventArgs args);
        void Subscribe(IStationService stationServ);
        void Unsubscribe(IStationService stationServ);
    }
}
