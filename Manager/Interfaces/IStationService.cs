using Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interfaces
{
    public interface IStationService
    {
        Station Station { get; set; }
        void MoveIn(Flight airplane);
        void MoveOut(IStationService stationServ);
    }
}
