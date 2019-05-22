using Common.Models;
using Manager.Interfaces;
using Manager.Models;
using Microsoft.AspNetCore.SignalR;
using Server.Models;
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

        public PhysicalStation[] GetAirportState() 
        {
            var stations = new PhysicalStation[3];

            var std1 = new Dictionary<FlightActionType, List<Station>>();
            std1[FlightActionType.Landing] = new List<Station>();
            var st1 = new PhysicalStation()
            {
                Height = 50,
                Width = 50,
                X = 100,
                Y = 100,
                NextStations = std1
            };

            var st2 = new PhysicalStation()
            {
                Height = 50,
                Width = 50,
                X = 200,
                Y = 200
            };

            var st3 = new PhysicalStation()
            {
                Height = 50,
                Width = 50,
                X = 150,
                Y = 310
            };
            
            std1[FlightActionType.Landing].Add(st2);
            std1[FlightActionType.Landing].Add(st3);
            
            stations[0] = st1;
            stations[1] = st2;
            stations[2] = st3;

            return stations;
        }

        protected override void Dispose(bool disposing)
        {
            _airportManager.RouteManager.OnAirplaneMoved -= OnAirPlaneMoved;
            base.Dispose(disposing);
        }

    }
}
