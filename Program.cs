// See https://aka.ms/new-console-template for more information
using FatestWalkingRoute_Version1;
using System.Diagnostics.Metrics;

string filePath0 = "Data/Central.txt";
string filePath1 = "Data/Bakerloo.txt";
string filePath2 = "Data/Circle.txt";
string filePath3 = "Data/District.txt";
string filePath4 = "Data/H&C.txt";
string filePath5 = "Data/Jubilee.txt";
string filePath6 = "Data/Metropolitan.txt";
string filePath7 = "Data/Northern.txt";
string filePath8 = "Data/Piccadilly.txt";
string filePath9 = "Data/Victoria.txt";
string filePath10 = "Data/Waterloo.txt";

string[] filePaths = new string[] { filePath0, filePath1, filePath2, filePath3, filePath4, filePath5, filePath6, filePath7, filePath8, filePath9, filePath10 };

TflUI tfLUI = new TflUI();
TfLApp tfLApp = new TfLApp();

TubeNetwork tubeNetwork = tfLUI.LoadData(filePaths);

TfLAppTester tester = new TfLAppTester(tubeNetwork, tfLApp);

while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Run tests");
    Console.WriteLine("2. Go to the main app");
    Console.WriteLine("3. Exit");

    int option;
    if (int.TryParse(Console.ReadLine(), out option))
    {
        switch (option)
        {
            case 1:
                tester.RunTests(); // Implement this method to run your tests
                break;
            case 2:
                tfLUI.ShowMenu(tubeNetwork);// Implement this method to run your main app
                break;
            case 3:
                Console.WriteLine("Exiting...");
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }
}



Console.ReadLine();