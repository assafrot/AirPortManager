using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IAirportStateLoader
    {
        AirportState Load();
        Dictionary<AirplaneActionType, IStationService> GetStartingPoints(AirportState state);
    }
}
