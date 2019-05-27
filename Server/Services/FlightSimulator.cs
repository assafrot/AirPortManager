using Common.Models;
using Manager.Interfaces;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class FlightSimulator : IFlightSimulator
    {
        public FlightSimulator(ITimer timer, IAirportManager airportManager)
        {
            _timer = timer;
            _airportManager = airportManager;
        }
        ITimer _timer;
        IAirportManager _airportManager;
        bool running;
        public async void Start()
        {
            running = true;
            while (running)
            {
                await _timer.Wait(1000);
                _airportManager.PushFlight(new Common.Models.Flight()
                {
                    ActionType = FlightActionType.Landing,
                    Model = "bla",
                    RequestedTime = DateTime.UtcNow,
                });
            }
        }

        public void Stop()
        {
            running = false;
        }
    }
}
