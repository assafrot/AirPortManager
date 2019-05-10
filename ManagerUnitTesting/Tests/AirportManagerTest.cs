using LiveTesting.LogicObjects;
using Manager.Interfaces;
using Manager.LogicObjects;
using Manager.Models;
using ManagerUnitTesting.Moqs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerUnitTesting.Tests
{
    class AirportManagerTest : Test
    {

        public void AirportManager_LoadAndPushTest()
        {
            IRouteManager router = new RouteManager();
            IAirportStateLoader loader = new AirportStateLoaderMoq(router);

            IAirportManager manager = new AirportManager(router, loader);

            manager.PushAirplane(new Flight() { ActionType = FlightActionType.Landing });
            manager.PushAirplane(new Flight() { ActionType = FlightActionType.Landing });

        }

    }
}
