﻿using DAL.Interfaces;
using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IAirportStateArchiver
    {
        IUnitOfWork UnitOfWork { get;}
        void StartArchiving();
    }
}
