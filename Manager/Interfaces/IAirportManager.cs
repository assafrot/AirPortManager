using Common.Models;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IAirportManager
    {
        IRouteManager RouteManager { get; set; }
        AirportState AirportState { get; set; }
        void PushFlight(Flight airplane);
    }
}
