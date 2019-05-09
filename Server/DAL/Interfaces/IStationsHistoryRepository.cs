﻿using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL.Interfaces
{
    interface IStationsHistoryRepository : IRepository<StationEventDB>
    {
    }
}