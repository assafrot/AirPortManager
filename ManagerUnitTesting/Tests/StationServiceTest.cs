using LiveTesting.LogicObjects;
using Manager.Interfaces;
using Manager.LogicObjects;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerUnitTesting.Tests
{
    public class StationServiceTest : Test
    {

        public void StationService_CreateTest()
        {
            RouteManager manager = new RouteManager();
            Station st = new Station();

            st.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st.NextStations[FlightActionType.Landing] = new List<IStationService>();
            st.NextStations[FlightActionType.Takeoff] = new List<IStationService>();

            IStationService ss = new StationService(st, manager);

        }

        public void StationService_MoveInAndOutTest()
        {
            RouteManager manager = new RouteManager();

            Station st = new Station();
            st.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st.NextStations[FlightActionType.Landing] = new List<IStationService>();
            st.NextStations[FlightActionType.Takeoff] = new List<IStationService>();

            IStationService ss = new StationService(st, manager);

            Station st2 = new Station();
            st2.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st2.NextStations[FlightActionType.Landing] = new List<IStationService>();
            st2.NextStations[FlightActionType.Takeoff] = new List<IStationService>();

            IStationService ss2 = new EndStationService(st2, manager);
            st.NextStations[FlightActionType.Landing].Add(ss2);
            Flight flight = new Flight();
            flight.ActionType = FlightActionType.Landing;

            ss.MoveIn(flight);
            int gotAirplaneMoved = 0;

            manager.OnAirplaneMovedOut += (station) =>
            {
                if (gotAirplaneMoved != 2)
                {
                    Assert(station == st);
                    gotAirplaneMoved++;
                }
            };

            manager.OnAirplaneMovedIn += (station) =>
            {
                if (gotAirplaneMoved != 2)
                {
                    Assert(station == st2);
                    gotAirplaneMoved++;
                }
            };

            while (gotAirplaneMoved != 2) { }

        }

        public void StationService_StartStationTest()
        {

            RouteManager manager = new RouteManager();

            Station st = new Station();
            st.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st.NextStations[FlightActionType.Landing] = new List<IStationService>();
            st.NextStations[FlightActionType.Takeoff] = new List<IStationService>();

            StartStationService ss = new StartStationService(st, manager);

            Station st2 = new Station();
            st2.NextStations = new Dictionary<FlightActionType, List<IStationService>>();
            st2.NextStations[FlightActionType.Landing] = new List<IStationService>();
            st2.NextStations[FlightActionType.Takeoff] = new List<IStationService>();

            IStationService ss2 = new EndStationService(st2, manager);
            st.NextStations[FlightActionType.Landing].Add(ss2);

            ss.MoveIn(new Flight() { ActionType = FlightActionType.Landing });
            ss.MoveIn(new Flight() { ActionType = FlightActionType.Landing });

        }


    }
}
