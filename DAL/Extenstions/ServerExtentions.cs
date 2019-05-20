using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using DAL.Interfaces;
using DAL;
using DAL.Repositories;

namespace Extenstions
{
    public static class ServerExtentions
    {
        public static StationEventDB ToDB (this StationEvent stationEvent)
        {
            return new StationEventDB()
            {
                Flight = stationEvent.Flight,
                EventType = stationEvent.EventType,
                Station = stationEvent.Station.ToDB(),
                Time = stationEvent.Time
            };
        }

        public static StationEvent ToDTO (this StationEventDB stationEventDb)
        {
            return new StationEvent()
            {
                Flight = stationEventDb.Flight,
                EventType = stationEventDb.EventType,
                Station = stationEventDb.Station.ToDTO(),
                Time = stationEventDb.Time
            };
        }

        public static StationDB ToDB (this Station station)
        {
            var stationDB = new StationDB()
            {
                Id = station.Id,
                Flight = station.Flight,
                EndPoint = station.EndPoint,
                StartPoint = station.StartPoint,                
            };
            return stationDB;
        }

        public static Station ToDTO(this StationDB stationDB)
        {
            var station = new Station()
            {
                Id = stationDB.Id,
                Flight = stationDB.Flight,
                EndPoint = stationDB.EndPoint,
                StartPoint = stationDB.StartPoint,                
            };

            return station;
        }
    }
}
