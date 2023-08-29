using System;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            new SqlConnectionPulse().Run();

            Console.WriteLine("END of program.");
        }
    }
}