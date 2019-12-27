using System;
using System.Linq;
using System.Reflection;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            int dayNR = 22;//DateTime.Today.Day;
            Day day = (Day)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == "Day"+ dayNR));

            //day.Part1();
            //day.Part2();
            Console.WriteLine("PART 1");
            Console.WriteLine(day.Part1());
            Console.WriteLine("PART 2");
            Console.WriteLine(day.Part2());
            Console.ReadLine();
        }
    }
}
