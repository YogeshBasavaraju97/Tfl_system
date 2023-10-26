using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatestWalkingRoute_Version1
{
    public class TflUI
    {
        public static TfLApp tflApp;

        public TflUI()
        {
            tflApp = new TfLApp();
        }

        public void Start(TubeNetwork tubeNetwork)
        {
            //LoadData(filePath); // Load the data
            ShowMenu(tubeNetwork); // Display the main menu
        }

        public void ShowMenu(TubeNetwork tubeNetwork)
        {
            // Display options to the user
            // Prompt for input and call the appropriate methods based on user's selection
            // Prompt for user type (customer or manager)
            Console.WriteLine("Welcome to Tfl Transport Console App");
            Console.WriteLine("Are you a customer or a manager?");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Manager");
            Console.WriteLine("Enter your choice:");

            string userTypeChoice = Console.ReadLine();
            string[] managerOptions = { "1", "2", "3", "5", "6", "7" }; // Manager options
            string[] customerOptions = { "4", "7" }; // Customer options

            bool isManager = userTypeChoice == "2";
            while (true)
            {
                // Display options based on user type
                if (isManager)
                {
                    // Manager options are not supported in Version 1
                    Console.WriteLine("Manager options are not supported in Version 1.");
                    Console.WriteLine("Press Enter to Continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("4. Find fastest route between two stations");
                    Console.WriteLine("7. Exit");

                    Console.WriteLine("Enter your choice:");
                    string choice = Console.ReadLine();

                    // Check if user choice is valid based on user type
                    if (!customerOptions.Contains(choice))
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                    }

                    switch (choice)
                    {
                        case "4":
                            int startStationIndex = GetStationIndexFromUser(tubeNetwork,"1st");
                            int endStationIndex = GetStationIndexFromUser(tubeNetwork,"2nd");

                            int[] previousStations = tflApp.FindShortestPath(tubeNetwork, startStationIndex, endStationIndex);
                            DisplayShortestPath(tubeNetwork,previousStations, startStationIndex, endStationIndex);

                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                            break;

                        case "7":
                            Console.WriteLine("Exiting...");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        public static void DisplayShortestPath(TubeNetwork tubeNetwork, int[] previousStations, int startStationIndex, int endStationIndex)
        {
            Console.WriteLine($"Shortest path from {tubeNetwork.Stations[startStationIndex].Name} to {tubeNetwork.Stations[endStationIndex].Name}:");
            Console.WriteLine($"(1) Start: {tubeNetwork.Stations[startStationIndex].Name}");
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
                Console.WriteLine($"({stepCounter}) {tubeNetwork.Stations[stationIndex1].Name} to {tubeNetwork.Stations[stationIndex2].Name} {tubeNetwork.AdjacencyMatrix[stationIndex1][stationIndex2]} min");
                stepCounter++;
            }
            Console.WriteLine($"({stepCounter}) End: {tubeNetwork.Stations[endStationIndex].Name}");
            Console.WriteLine($"Total Journey Time: {totalTime} minutes");
        }

        public static int GetStationIndexFromUser(TubeNetwork tubeNetwork,string argument)
        {
            Console.WriteLine($"Enter {argument} station name:");
            string stationName = Console.ReadLine();
            int stationIndex = tflApp.GetStationIndexByName(stationName,tubeNetwork);
            if (stationIndex == -1)
            {
                Console.WriteLine("Station not found. Please try again.");
            }
            return stationIndex;
        }

        public TubeNetwork LoadData(string[] filePaths)
        {
            TubeNetwork tubeNetwork = new TubeNetwork(200);
            List<Station> stationList = new List<Station>();

            foreach (string filePath in filePaths)
            {
                int lineCount = File.ReadAllLines(filePath).Length;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line) || line.Contains("\t")) continue;
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                        string tubeLine = fileNameWithoutExtension;

                        // Parsing line1
                        string[] line1Parts = line.Split(',');
                        string station1Name = line1Parts[0].Trim();

                        string[] stationB2Parts = line1Parts[1].Trim().Split(' ');
                        string station2Name = string.Join(" ", stationB2Parts.Take(stationB2Parts.Length - 1)).Trim();

                        int time = int.Parse(stationB2Parts[stationB2Parts.Length - 1].Replace("mins", "").Trim());

                        Station station1 = stationList.FirstOrDefault(s => s.Name == station1Name);
                        Station station2 = stationList.FirstOrDefault(s => s.Name == station2Name);

                        if (station1 == null)
                        {
                            station1 = new Station(station1Name);
                            stationList.Add(station1);
                        }

                        if (station2 == null)
                        {
                            station2 = new Station(station2Name);
                            stationList.Add(station2);
                        }

                        int station1Index = stationList.IndexOf(station1);
                        int station2Index = stationList.IndexOf(station2);

                        tubeNetwork.AdjacencyMatrix[station1Index][station2Index] = time;
                        tubeNetwork.AdjacencyMatrix[station2Index][station1Index] = time;
                    }
                }
            }

            tubeNetwork.Stations = stationList.ToArray();
            return tubeNetwork;
        }


        public TubeNetwork LoadData(string filePath)
        {   
            int lineCount = File.ReadAllLines(filePath).Length;
            TubeNetwork tubeNetwork = new TubeNetwork(lineCount);
            Station[] stationArray = new Station[lineCount];
            int stationCount = 0;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || line.Contains("\t")) continue;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string tubeLine = fileNameWithoutExtension;
                    // Parsing line1
                    string[] line1Parts = line.Split(',');
                    string station1Name = line1Parts[0].Trim();

                    string[] stationB2Parts = line1Parts[1].Trim().Split(' ');
                    string station2Name = string.Join(" ", stationB2Parts.Take(stationB2Parts.Length - 1)).Trim();

                    int time = int.Parse(stationB2Parts[stationB2Parts.Length - 1].Replace("mins", "").Trim());

                    Station station1 = null;
                    Station station2 = null;

                    for (int i = 0; i < stationCount; i++)
                    {
                        if (stationArray[i].Name == station1Name) station1 = stationArray[i];
                        if (stationArray[i].Name == station2Name) station2 = stationArray[i];
                    }

                    if (station1 == null)
                    {
                        station1 = new Station(station1Name);
                        stationArray[stationCount++] = station1;
                    }

                    if (station2 == null)
                    {
                        station2 = new Station(station2Name);
                        stationArray[stationCount++] = station2;
                    }

                    int station1Index = Array.IndexOf(stationArray, station1);
                    int station2Index = Array.IndexOf(stationArray, station2);

                    tubeNetwork.AdjacencyMatrix[station1Index][station2Index] = time;
                    tubeNetwork.AdjacencyMatrix[station2Index][station1Index] = time;
                }
            }

            Station[] finalStationArray = new Station[stationCount];
            Array.Copy(stationArray, finalStationArray, stationCount);
            tubeNetwork.Stations = finalStationArray;
            return tubeNetwork;
        }


    }
}
