using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IStationServiceBuilder
    {
        IStationServiceBuilder AddStation(Station station);
        IStationServiceBuilder AddNextStationService(FlightActionType actionType, IStationService stationService);
        Station GetStation();
        IStationService GetStationService();
        bool IsStationMatchService(Station station);
    }
}
