﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IDBSeederService
    {
        void JsonSeed(string jsonData);
    }
}