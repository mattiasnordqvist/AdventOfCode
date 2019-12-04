using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day3 : Day
    {
        public Day3()
        {
            AddTestForPart1(
                @"
R75,D30,R83,U83,L12,D49,R71,U7,L72
U62,R66,U55,R34,D71,R55,D58,R83
", "159");
            AddTestForPart1(
              @"
R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7
", "135");

            AddTestForPart2(@"R75,D30,R83,U83,L12,D49,R71,U7,L72
U62, R66, U55, R34, D71, R55, D58, R83", "610");

            AddTestForPart2(@"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", "410");
        }
        protected override string Part1Code(string data)
        {
            Dictionary<(int x, int y), HashSet<int>> paths = new Dictionary<(int, int), HashSet<int>>();
            List<(int x, int y)> crossSections = new List<(int x, int y)>();
            var wireNr = 1;
            foreach (var line in data.Lines())
            {
                var pos = (x: 0, y: 0);
                foreach (var instruction in line.FromCommaSep())
                {
                    var direction = instruction[0];
                    var steps = int.Parse(instruction.Substring(1));

                    for (int i = 0; i < steps; i++)
                    {
                        if (direction == 'R')
                        {
                            pos = (pos.x + 1, pos.y);
                        }
                        if (direction == 'L')
                        {
                            pos = (pos.x - 1, pos.y);
                        }
                        if (direction == 'U')
                        {
                            pos = (pos.x, pos.y + 1);
                        }
                        if (direction == 'D')
                        {
                            pos = (pos.x, pos.y - 1);
                        }
                        if (!paths.ContainsKey(pos))
                        {
                            paths.Add(pos, new HashSet<int> { wireNr });
                        }
                        else
                        {
                            paths[pos].Add(wireNr);
                            if (paths[pos].Count >= 2)
                            {
                                crossSections.Add(pos);
                            }
                        }
                    }
                }
                wireNr++;
            }

            int minDistance = crossSections.Min(c => Math.Abs(c.x) + Math.Abs(c.y));
            return minDistance.ToString();
        }

        protected override string Part2Code(string data)
        {
            Dictionary<(int x, int y), Dictionary<int, int>> paths = new Dictionary<(int x, int y), Dictionary<int, int>>();
            List<int> crossSectionSteps = new List<int>();
            var wireNr = 1;

            foreach (var line in data.Lines())
            {
                var steps = 0;
                var pos = (x: 0, y: 0);
                foreach (var instruction in line.FromCommaSep())
                {
                    var direction = instruction[0];
                    var count = int.Parse(instruction.Substring(1));

                    for (int i = 0; i < count; i++)
                    {
                        steps++;
                        if (direction == 'R')
                        {
                            pos = (pos.x + 1, pos.y);
                        }
                        if (direction == 'L')
                        {
                            pos = (pos.x - 1, pos.y);
                        }
                        if (direction == 'U')
                        {
                            pos = (pos.x, pos.y + 1);
                        }
                        if (direction == 'D')
                        {
                            pos = (pos.x, pos.y - 1);
                        }
                        if (!paths.ContainsKey(pos))
                        {
                            paths.Add(pos, new Dictionary<int, int> { { wireNr, steps } });
                        }
                        else
                        {
                            if (paths[pos].Count < 2)
                            {
                                if (!paths[pos].ContainsKey(wireNr))
                                {
                                    paths[pos].Add(wireNr, steps);
                                }
                                if (paths[pos].Count >= 2)
                                {
                                    crossSectionSteps.Add(paths[pos].Select(x => x.Value).Sum());
                                }
                            }
                        }
                    }
                }
                wireNr++;
            }
            return crossSectionSteps.Min().ToString();
        }
    }
}
