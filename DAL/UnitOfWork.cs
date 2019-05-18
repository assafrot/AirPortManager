using DAL.Interfaces;
using DAL.Repositories;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AirportDbContext context)
        {
            _context = context;
            Stations = new RealTimeStationRepository(context);
            StationHistory = new StationsHistoryRepository(context);
            Flights = new FlightsRepository(context);
        }

        private readonly AirportDbContext _context;

        public IRealTimeStationRepository Stations { get; private set; }

        public IStationsHistoryRepository StationHistory { get; private set; }

        public IFlightsRepository Flights { get; private set; }

        public IStationsLinksRepository StationsLinks { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
