﻿using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using Microsoft.AspNetCore.SignalR;
using Server.Interfaces;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    
    public class AirportHub : Hub
    {

        public AirportHub(IAirportManager airportManager, IPhysicalStationBuilder physicalStationBuilder)
        {
            _airportManager = airportManager;
            _physicalStationBuilder = physicalStationBuilder;
            _airportManager.RouteManager.OnAirplaneMoved += OnAirPlaneMoved;
        }

        IAirportManager _airportManager;
        IPhysicalStationBuilder _physicalStationBuilder;

        void OnAirPlaneMoved(StationEvent stationEvent)
        {
            Clients.Caller.SendAsync("OnAirplaneMove", stationEvent);
        }

        public List<PhysicalStation> GetAirportState() 
        {
            return _physicalStationBuilder.GetPhysicalStations();
        }

        protected override void Dispose(bool disposing)
        {
            _airportManager.RouteManager.OnAirplaneMoved -= OnAirPlaneMoved;
            base.Dispose(disposing);
        }

    }
}
