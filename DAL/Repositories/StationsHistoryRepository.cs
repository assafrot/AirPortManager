using DAL;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StationsHistoryRepository: Repository<StationEventDB>, IStationsHistoryRepository
    {
        public StationsHistoryRepository(AirportDbContext context) : base(context)
        {
        }
    }
}
