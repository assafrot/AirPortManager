using Common.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IPhysicalStationBuilder
    {
        List<Station> GetPhysicalStations();
    }
}
