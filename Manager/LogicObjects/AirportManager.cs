using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    public class AirportManager : IAirportManager
    {

        public AirportState AirportState { get; set; }

        public event Action AirplaneMoved;

        public void PushAirplane(Airplane airplane)
        {
            var successed = TryToGetInAirport(airplane);
            AirportState.AirplanesInQueue[airplane.ActionType].Add(airplane);

        }

        public bool TryToGetInAirport(Airplane airplane)
        {
            foreach (var station in AirportState.Stations)
            {
                if(station.StartPoint && station.NextStations.ContainsKey(airplane.ActionType))
                {
                    var successed = TryToMoveAirplane(airplane, station);

                    if (successed)
                    {
                        return true;
                    }
                } 
            }

            return false;
        }

        public bool TryToMoveAirplane(Airplane airplane, Station station)
        {
            return false;
        }


    }
}
