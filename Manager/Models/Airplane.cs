using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public AirplaneActionType ActionType { get; set; }
    }
}
