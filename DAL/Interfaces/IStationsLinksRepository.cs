using Common.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IStationsLinksRepository : IRepository<StationsLinks>
    {
        Dictionary<FlightActionType, List<Station>> GetLinkedStation(int stationId);
    }
}
