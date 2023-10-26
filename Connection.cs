using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class Connection
    {
        public int StationIndex { get; set; }
        public int Time { get; set; }
        public string TubeLine { get; set; }
        public int OriginalTime { get; set; }
        public Status Status { get; set; }
        public string StatusReason { get; set; }

        public Connection(int stationIndex, int time, string tubeLine)
        {
            StationIndex = stationIndex;
            Time = time;
            TubeLine = tubeLine;
            OriginalTime = time;
            Status = Status.Open;
            StatusReason = "";
        }
    }
}
