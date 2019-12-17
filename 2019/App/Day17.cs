using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day17 : Day
    {
        protected override string Part1Code(string data)
        {
            return "";
            var program = IntCodeComputer.ParseProgram(data);
            var computer = new IntCodeComputer { Memory = program };
            var p = (x: 0, y: 0);
            var map = new HashSet<(int, int)>();
            var intersections = new List<(int x, int y)>();
            computer.WriteOutput = x =>
            {
                if ((char)x == '#')
                {
                    map.Add(p);
                }
                if (map.Contains((p.x, p.y)) &&
                    map.Contains((p.x, p.y - 1)) &&
                    map.Contains((p.x - 1, p.y - 1)) &&
                    map.Contains((p.x + 1, p.y - 1)) &&
                    map.Contains((p.x, p.y - 2)))
                {
                    intersections.Add((p.x, p.y - 1));
                }

                if (x == 10)
                {
                    p = (0, p.y + 1);
                }
                else
                {
                    p = (p.x + 1, p.y);
                }

            };

            computer.Compute();

            foreach (var b in map)
            {
                Console.SetCursorPosition(b.Item1, b.Item2);
                Console.Write('#');
            }
            var sum = intersections.Sum(x => x.x * x.y);
            return sum.ToString();
        }


        protected override string Part2Code(string data)
        {
            Console.Clear();
            var program = IntCodeComputer.ParseProgram(data);
            program[0] = 2;
            var computer = new IntCodeComputer { Memory = program };
            var mainRoutine = "A,A,C,B,B,A,B,C,B,C".Select(x => (int)x).Append(10);
            var aRoutine = "L,4,L,4,L,6,R,10,L,6".Select(x => (int)x).Append(10);
            var bRoutine = "R,8,R,10,L,6".Select(x => (int)x).Append(10);
            var cRoutine = "L,12,L,6,R,10,L,6".Select(x => (int)x).Append(10);
            var noVideo = "n".Select(x => (int)x).Append(10);

            var input = mainRoutine.Concat(aRoutine).Concat(bRoutine).Concat(cRoutine)
                .Concat(noVideo).ToArray();
            int i = 0;
            computer.ReadInput = () =>
            {
                return input[i++];
            };
            computer.WriteOutput = (x) => {};
            
            var dust = computer.Compute();
            return dust.ToString();
        }
    }
}
