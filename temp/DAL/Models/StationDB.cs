﻿using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL.Models
{
    public class StationDB : Station
    {
        public int FlightId { get; set; }
    }
}