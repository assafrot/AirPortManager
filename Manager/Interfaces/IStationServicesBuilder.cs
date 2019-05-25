using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IStationServicesBuilder
    {
        Dictionary<FlightActionType, IStationService> StartingStations { get; set; }
        List<IStationService> BuildServices(List<Station> stations);
    }
}
