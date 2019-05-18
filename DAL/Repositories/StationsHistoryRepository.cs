using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class StationsHistoryRepository: Repository<StationEventDB>, IStationsHistoryRepository
    {
        public StationsHistoryRepository(AirportDbContext context) : base(context)
        {
        }
    }
}
