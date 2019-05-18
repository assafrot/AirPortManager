using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{


    public class RealTimeStationRepository : Repository<StationDB> , IRealTimeStationRepository
    {
        public RealTimeStationRepository(AirportDbContext context) :base(context) {}

  
    }
}
