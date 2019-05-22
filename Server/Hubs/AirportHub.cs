using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    
    public class AirportHub : Hub
    {

        public AirportHub(IAirportManager airportManager)
        {
            _airportManager = airportManager;
            _airportManager.RouteManager.OnAirplaneMoved += OnAirPlaneMoved;
        }

        IAirportManager _airportManager;

        void OnAirPlaneMoved(StationEvent station)
        {
            Clients.Caller.SendAsync("OnAirplaneMove", station);
        }

        public string Test() 
        {
            return "test string";
        }

        protected override void Dispose(bool disposing)
        {
            _airportManager.RouteManager.OnAirplaneMoved -= OnAirPlaneMoved;
            base.Dispose(disposing);
        }

    }
}
