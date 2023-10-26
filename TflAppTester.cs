using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class TfLAppTester
    {
        private TfLApp tflApp;
        private TubeNetwork tubeNetwork;

        public TfLAppTester(TubeNetwork _tubeNetwork, TfLApp tflApp)
        {
            this.tubeNetwork = _tubeNetwork;
            this.tflApp = tflApp;
        }


        public void RunTests()
        {
            // Move the content of the Main method here
            // ...
            // Initialize stations and connections data
            // ...
            TfLApp tflApp = new TfLApp();
            List<(string, string)> testCases = new List<(string, string)>
            {
                ("Elephant and Castle", "Charing Cross"),
                ("Elephant and Castle", "Marylebone"),
                ("Edgware Road (Circle Line)", "Aldgate East"),
                // Add more test cases as needed
            };

            // Consistency tests
            Console.WriteLine("Consistency Test Results:");
            Console.WriteLine("Start Station | End Station | Fastest Time");
            Console.WriteLine("------------------------------------------");
            foreach (var testCase in testCases)
            {
                // Find the shortest path and calculate the total time
                int startStationIndex = tflApp.GetStationIndexByName(testCase.Item1, tubeNetwork);
                int endStationIndex = tflApp.GetStationIndexByName(testCase.Item2, tubeNetwork);
                int[] path = tflApp.FindShortestPath(tubeNetwork, startStationIndex,endStationIndex);
                int totalTime = CalculateTotalTime(tubeNetwork, path, startStationIndex, endStationIndex);
                Console.WriteLine($"{testCase.Item1} | {testCase.Item2} | {totalTime}");
            }
            Console.WriteLine();

            // Benchmarking tests
            Console.WriteLine("Benchmarking Test Results:");
            Console.WriteLine("Start Station | End Station | Average Execution Time (ms)");
            Console.WriteLine("-------------------------------------------------------");
            foreach (var testCase in testCases)
            {
                int runs = 10;
                double averageExecutionTime = Benchmark(tubeNetwork, tflApp.GetStationIndexByName(testCase.Item1, tubeNetwork), tflApp.GetStationIndexByName(testCase.Item2, tubeNetwork), runs);
                Console.WriteLine($"{testCase.Item1} | {testCase.Item2} | {averageExecutionTime}");
            }
            Console.ReadLine();
            Console.Clear();
        }
       

         
        public double Benchmark(TubeNetwork tubeNetwork, int startIndex,int endIndex, int runs)
        {
            Stopwatch stopwatch = new Stopwatch();
            double totalTime = 0;

            for (int i = 0; i < runs; i++)
            {
                stopwatch.Restart();
                tflApp.FindShortestPath(tubeNetwork, startIndex, endIndex);
                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;
            }

            return totalTime / runs;
        }





        public void RunBenchmarkTests()
        {
            string[] startStations = { "Edgware Road (Circle Line)" }; // Replace these with actual station names.
            string[] endStations = { "Bayswater" }; // Replace these with actual station names.

            Console.WriteLine("Benchmark results for version 1:");
            Console.WriteLine("Start Station\tEnd Station\tExecution Time (ms)");

            for (int i = 0; i < startStations.Length; i++)
            {
                for (int j = 0; j < endStations.Length; j++)
                {
                    int startIndex = tflApp.GetStationIndexByName(startStations[i], tubeNetwork);
                    int endIndex = tflApp.GetStationIndexByName(endStations[j], tubeNetwork);

                    if (startIndex != -1 && endIndex != -1)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        tflApp.FindShortestPath(tubeNetwork, startIndex, endIndex);
                        stopwatch.Stop();

                        Console.WriteLine($"{startStations[i]}\t{endStations[j]}\t{stopwatch.ElapsedMilliseconds}");
                    }
                }
            }
        }
        public static int CalculateTotalTime(TubeNetwork tubeNetwork, int[] previousStations, int startStationIndex, int endStationIndex)
        {
           // Console.WriteLine($"Shortest path from {tubeNetwork.Stations[startStationIndex].Name} to {tubeNetwork.Stations[endStationIndex].Name}:");
           // Console.WriteLine($"(1) Start: {tubeNetwork.Stations[startStationIndex].Name}");
            int totalTime = 0;
            int currentIndex = endStationIndex;
            Stack<int> path = new Stack<int>();

            while (currentIndex != startStationIndex)
            {
                path.Push(currentIndex);
                int prevIndex = previousStations[currentIndex];
                int time = tubeNetwork.AdjacencyMatrix[prevIndex][currentIndex];
                totalTime += time < 0 ? 0 : time; // Ensure that the time is not negative
                currentIndex = prevIndex;
            }
            path.Push(startStationIndex);

            int stepCounter = 2;
            while (path.Count > 1)
            {
                int stationIndex1 = path.Pop();
                int stationIndex2 = path.Peek();
            //    Console.WriteLine($"({stepCounter}) {tubeNetwork.Stations[stationIndex1].Name} to {tubeNetwork.Stations[stationIndex2].Name} {tubeNetwork.AdjacencyMatrix[stationIndex1][stationIndex2]} min");
                stepCounter++;
            }
            // Console.WriteLine($"({stepCounter}) End: {tubeNetwork.Stations[endStationIndex].Name}");
            // Console.WriteLine($"Total Journey Time: {totalTime} minutes");
            return totalTime;
        }

    }
}
