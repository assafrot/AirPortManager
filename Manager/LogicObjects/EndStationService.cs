﻿using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class EndStationService : IStationService
    {

        public EndStationService(Station station, IRouteManager routeManager)
        {
            Station = station;
            _routeManager = routeManager;
        }

        public Station Station { get; set; }
        IRouteManager _routeManager;

        public void MoveIn(Flight airplane)
        {
            _routeManager.NotifyStationEmptied(new StationEmptiedEventArgs(this));
        }

        public void MoveOut(IStationService stationServ)
        {
            throw new Exception("Cant Move Out From End Point Station");
        }
    }
}
