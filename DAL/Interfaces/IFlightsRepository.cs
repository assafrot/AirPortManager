﻿using Common.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IFlightsRepository : IRepository<Flight>
    {
        IEnumerable<Flight> GetAllFutureFlights(FlightActionType flightActionType);
    }
}