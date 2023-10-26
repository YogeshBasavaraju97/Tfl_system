using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class Station
    {
        public string Name { get; set; }
        public Connection[] Connections { get; set; }

        public Station(string name)
        {
            Name = name;        
        }
        public Station(string name, int numberOfStations)
        {
            Name = name;
            Connections = new Connection[numberOfStations];
        }
    }

    public enum Status
    {
        Open,
        Closed,
        Delayed
    }

}
