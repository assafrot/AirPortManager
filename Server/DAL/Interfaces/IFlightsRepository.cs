﻿using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL.Interfaces
{
    public interface IFlightsRepository : IRepository<FlightDB>
    {
        //make request by FlightActionType enum.
        IEnumerable<FlightDB> GetAllFutureLandings();
        IEnumerable<FlightDB> GetAllFutureTakeOffs();
    }
}
