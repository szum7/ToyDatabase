using ConsoleApp._230830_SelectInBatch;
using ConsoleApp.SelectInBatch;
using System;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //new SqlConnectionPulse().Run();
            //new GenerateInserts().RunV2();
            new RunInBatch().Run();

            Console.WriteLine("END of program.");
        }
    }
}