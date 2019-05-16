using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{


    public class RealTimeStationRepository : Repository<StationDB> , IRealTimeStationRepository
    {
        public RealTimeStationRepository(AirportDbContext context) :base(context) {}
    }
}
