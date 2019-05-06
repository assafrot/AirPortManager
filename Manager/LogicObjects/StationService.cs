using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    class StationService : IStationService
    {

        public StationService(Station station, IEventManager eventManager)
        {
            _eventManager = eventManager;
            Station = station;
        }
        IEventManager _eventManager;

        public Station Station { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void MoveIn(Airplane airplane)
        {
            Station.Airplane = airplane;   
        }

        public void MoveOut(IStationService station)
        {
            station.MoveIn(Station.Airplane);
            Station.Airplane = null;
            _eventManager.Emit(new StationEmptiedEventArgs(station));
        }

    }
}
