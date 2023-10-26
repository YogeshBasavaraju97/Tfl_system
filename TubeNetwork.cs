using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class TubeNetwork
    {
        public Station[] Stations { get;  set; }
        public int[][] AdjacencyMatrix { get; private set; }

        public TubeNetwork() { }

        public TubeNetwork(int numberOfStations)
        {
            Stations = new Station[numberOfStations];
            AdjacencyMatrix = new int[numberOfStations][];

            for (int i = 0; i < numberOfStations; i++)
            {
                AdjacencyMatrix[i] = new int[numberOfStations];
                for (int j = 0; j < numberOfStations; j++)
                {
                    AdjacencyMatrix[i][j] = i == j ? 0 : int.MaxValue;
                }
            }
        }

        public void AddStation(int index, string name)
        {
            Stations[index] = new Station(name, Stations.Length);
        }

        public void AddConnection(int stationAIndex, int stationBIndex, int time, string tubeLine)
        {
            AdjacencyMatrix[stationAIndex][stationBIndex] = time;
            AdjacencyMatrix[stationBIndex][stationAIndex] = time;
            Stations[stationAIndex].Connections[stationBIndex] = new Connection(stationBIndex, time, tubeLine);
            Stations[stationBIndex].Connections[stationAIndex] = new Connection(stationAIndex, time, tubeLine);
        }

    }

}
