using Manager.Interfaces;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.LogicObjects
{
    class EndStationService : IStationService
    {
        public Station Station { get; set; }

        public void MoveIn(Flight airplane)
        {
            
        }

        public void MoveOut(IStationService stationServ)
        {
            throw new Exception("Cant Move Out From End Point Station");
        }
    }
}
