using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day3 : Day
    {
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
