using Common.Models;
using DAL.Interfaces;
using DAL.Models;
using Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{


    public class RealTimeStationRepository : Repository<StationDB> , IRealTimeStationRepository
    {
        public RealTimeStationRepository(AirportDbContext context) :base(context) {}
        public AirportDbContext AirportContext { get { return _context as AirportDbContext; } }

        public Dictionary<FlightActionType, List<Station>> GetLinkedStation(int stationId)
        {
            var linkedStations = new Dictionary<FlightActionType, List<Station>>();
            var landingStationLinks = AirportContext.StationsLinks.Where(sl => sl.OriginId == stationId
            && sl.ActionType == FlightActionType.Landing);
            var takeoffStationLinks = AirportContext.StationsLinks.Where(sl => sl.OriginId == stationId
          && sl.ActionType == FlightActionType.Takeoff);
            if (landingStationLinks.Any())
            {
                linkedStations[FlightActionType.Landing] = new List<Station>();
            }
            foreach (var link in landingStationLinks)
            {
                var stationToAdd = AirportContext.Stations.Find(link.DestinationId).ToDTO();
                linkedStations[FlightActionType.Landing].Add(stationToAdd);
            }
            if (takeoffStationLinks.Any())
            {
                linkedStations[FlightActionType.Takeoff] = new List<Station>();
            }
            foreach (var link in takeoffStationLinks)
            {
                var stationToAdd = AirportContext.Stations.Find(link.DestinationId).ToDTO();
                linkedStations[FlightActionType.Takeoff].Add(stationToAdd);
            }
            return linkedStations;
        }
    }
}
