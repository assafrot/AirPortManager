using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IAirportManager
    {
        AirportState AirportState { get; set; }
        event Action AirplaneMoved;
        void PushAirplane(Airplane airplane);
    }
}
