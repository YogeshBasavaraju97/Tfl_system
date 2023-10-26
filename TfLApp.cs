using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class TfLApp
    {
        public int[] FindShortestPath(TubeNetwork network, int startStationIndex, int endStationIndex)
        {
            int numberOfStations = network.Stations.Length;
            bool[] visited = new bool[numberOfStations];
            int[] distances = new int[numberOfStations];
            int[] previousStations = new int[numberOfStations];

            for (int i = 0; i < numberOfStations; i++)
            {
                distances[i] = i == startStationIndex ? 0 : int.MaxValue;
                previousStations[i] = -1;
            }

            for (int i = 0; i < numberOfStations; i++)
            {
                int currentStationIndex = -1;
                int minDistance = int.MaxValue;
                for (int j = 0; j < numberOfStations; j++)
                {
                    if (!visited[j] && distances[j] < minDistance)
                    {
                        minDistance = distances[j];
                        currentStationIndex = j;
                    }
                }

                if (currentStationIndex == -1 || currentStationIndex == endStationIndex)
                {
                    break;
                }

                visited[currentStationIndex] = true;

                // Loop over neighbors of the current station
                for (int j = 0; j < network.AdjacencyMatrix[currentStationIndex].Length; j++)
                {
                    // Check if there's a connection between the current station and the neighbor
                    if (network.AdjacencyMatrix[currentStationIndex][j] != int.MaxValue)
                    {
                        int alt = distances[currentStationIndex] + network.AdjacencyMatrix[currentStationIndex][j];
                        if (alt < distances[j])
                        {
                            distances[j] = alt;
                            previousStations[j] = currentStationIndex;
                        }
                    }
                }
            }

            return previousStations;
        }

        public int GetStationIndexByName(string stationName, TubeNetwork tubeNetwork)
        {
            for (int i = 0; i < tubeNetwork.Stations.Length; i++)
            {
                if (tubeNetwork.Stations[i].Name.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1; // Return -1 if the station name is not found
        }


    }
}
