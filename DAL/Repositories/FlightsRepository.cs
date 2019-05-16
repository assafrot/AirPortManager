using Common.Models;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FlightsRepository: Repository<Flight>, IFlightsRepository
    {
        public FlightsRepository(AirportDbContext context) : base(context) {}

        public AirportDbContext AirportContext { get { return _context as AirportDbContext; } }

        public IEnumerable<Flight> GetAllFutureFlights(FlightActionType flightActionType)
        {
            return AirportContext.Flights.Where(f => f.ActionType == flightActionType
           && f.InQueue);
        }

    }
}
