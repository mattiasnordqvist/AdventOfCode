using System;
using System.Linq;
using System.Reflection;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            int dayNR = DateTime.Today.Day;
            Day day = (Day)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == "Day"+ dayNR));
            
            Console.WriteLine("PART 1");
            Console.WriteLine(day.Part1());
            Console.WriteLine("PART 2");
            Console.WriteLine(day.Part2());
            Console.ReadLine();
        }
    }
}
