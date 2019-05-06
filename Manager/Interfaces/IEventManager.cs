using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IEventManager
    {
        void Emit(StationEmptiedEventArgs args);
        void Subscribe(IStationService stationServ);
        void Unsubscribe(IStationService stationServ);
    }
}
