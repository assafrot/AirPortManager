using Manager.Models;
using Server.DAL.Interfaces;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL.Repositories
{
    public class FlightsRepository: Repository<FlightDB>, IFlightsRepository
    {
        public FlightsRepository(AirportDbContext context) : base(context) {}

        public AirportDbContext AirportContext { get { return _context as AirportDbContext; } }

        public IEnumerable<FlightDB> GetAllFutureLandings()
        {
            return AirportContext.Flights.Where(f => f.ActionType == FlightActionType.Landing 
            &&  f.PlannedTime >= DateTime.UtcNow);
        }

        public IEnumerable<FlightDB> GetAllFutureTakeOffs()
        {
            return AirportContext.Flights.Where(f => f.ActionType == FlightActionType.Takeoff
            && f.PlannedTime >= DateTime.UtcNow);
        }
    }
}
