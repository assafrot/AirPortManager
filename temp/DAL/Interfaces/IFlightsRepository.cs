using Manager.Models;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL.Interfaces
{
    public interface IFlightsRepository : IRepository<Flight>
    {
        //make request by FlightActionType enum.
        IEnumerable<Flight> GetAllFutureFlights(FlightActionType flightActionType);
    }
}
