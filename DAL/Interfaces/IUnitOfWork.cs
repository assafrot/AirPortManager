using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRealTimeStationRepository Stations { get; }
        IStationsHistoryRepository StationHistory { get; }
        IFlightsRepository Flights { get; }
        IStationsLinksRepository StationsLinks { get; }

        void Commit();
    }
}
