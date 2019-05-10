using Manager.Interfaces;
using Manager.LogicObjects;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerUnitTesting.Moqs
{
    class AirportStateLoaderMoq : IAirportStateLoader
    {

        IRouteManager _routeManager;
        IStationService start;
        public AirportStateLoaderMoq(IRouteManager routeManager)
        {
            _routeManager = routeManager;
        }

        public Dictionary<FlightActionType, IStationService> GetStartingPoints(AirportState state)
        {
            Dictionary<FlightActionType, IStationService> startingPoints = new Dictionary<FlightActionType, IStationService>();

            startingPoints[FlightActionType.Landing] = start;

            return startingPoints;
        }

        public AirportState Load()
        {
            var aState = new AirportState();

            var st = new Station();
            st.StartPoint = true;
            st.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st.NextStations[FlightActionType.Landing] = new List<IStationService>();

            var st2 = new Station();
            st2.EndPoint = true;
            st2.NextStations = new Dictionary<FlightActionType, List<IStationService>>();

            aState.Stations = new List<Station>();
            aState.Stations.Add(st);
            aState.Stations.Add(st2);


            start = new StartStationService(st, _routeManager);
            var end = new EndStationService(st2, _routeManager);
            

            st.NextStations[FlightActionType.Landing].Add(end);

            return aState;
        }
    }
}
