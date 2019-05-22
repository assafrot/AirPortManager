

using Common.Models;

namespace Server.Models 
{
    public class PhysicalStation : Station
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}