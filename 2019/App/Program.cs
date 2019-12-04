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
            int i = 1;
            foreach(var test in day.part1Tests)
            {
                if(test.Item2 != day.Part1(test.Item1))
                {
                    Console.WriteLine($"Test {i} for part 1 failed");
                }
                else
                {
                    Console.WriteLine($"Part 1 test {i} succeeded");
                }
            }
            Console.WriteLine(day.Part1());
            Console.WriteLine("PART 2");
            i = 1;
            foreach (var test in day.part2Tests)
            {
                if (test.Item2 != day.Part2(test.Item1))
                {
                    Console.WriteLine($"Test {i} for part 2 failed");
                }
                else
                {
                    Console.WriteLine($"Part 2 test {i} succeeded");
                }
            }
            Console.WriteLine(day.Part2());
            Console.ReadLine();
        }
    }
}
