using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day20 : Day
    {
        protected override string Part1Code(string data)
        {
            HashSet<(int x, int y)> map = new HashSet<(int x, int y)>();
            Dictionary<(int x, int y), (int x, int y)> portals = new Dictionary<(int x, int y), (int x, int y)>();
            int y = 0;
            List<(int x, int y, string name)> portalsList = new List<(int x, int y, string name)>();
            var lines = data.Lines().ToArray();
            foreach (var line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                    {
                        map.Add((x, y));
                    }

                    if (x < line.Length - 2 && line[x] == '.' && char.IsLetter(line[x + 1]) && char.IsLetter(line[x + 2]))
                    {
                        portalsList.Add((x, y, line[x + 1] + "" + line[x + 2]));
                    }
                    if (x < line.Length - 2 && char.IsLetter(line[x]) && char.IsLetter(line[x + 1]) && line[x + 2] == '.')
                    {
                        portalsList.Add((x + 2, y, line[x] + "" + line[x + 1]));
                    }
                    if (y < lines.Length - 2 && line[x] == '.' && char.IsLetter(lines[y + 1][x]) && char.IsLetter(lines[y + 2][x]))
                    {
                        portalsList.Add((x, y, lines[y + 1][x] + "" + lines[y + 2][x]));
                    }
                    if (y < lines.Length - 2 && char.IsLetter(line[x]) && char.IsLetter(lines[y + 1][x]) && lines[y + 2][x] == '.')
                    {
                        portalsList.Add((x, y + 2, line[x] + "" + lines[y + 1][x]));
                    }
                }
                y++;
            }
            var start = Pos(portalsList.Single(x => x.name == "AA"));
            var end = Pos(portalsList.Single(x => x.name == "ZZ"));

            foreach (var pg in portalsList.GroupBy(x => x.name))
            {
                portals[Pos(pg.First())] = Pos(pg.Last());
                portals[Pos(pg.Last())] = Pos(pg.First());
            }
            var floodedMap = map.ToDictionary(x => x, x => int.MaxValue);
            floodedMap[start] = 0;
            Flood(floodedMap, portals, start);

            return floodedMap[end].ToString();
        }

        private static void Flood(Dictionary<(int x, int y), int> floodedMap, Dictionary<(int x, int y), (int x, int y)> portals, (int x, int y) pos)
        {
            var neighbours = new List<(int x, int y)> { (pos.x - 1, pos.y), (pos.x + 1, pos.y), (pos.x, pos.y - 1), (pos.x, pos.y + 1) };
            if (portals.ContainsKey(pos))
            {
                neighbours.Add(portals[pos]);
            }
            foreach (var neighbour in neighbours)
            {
                if (floodedMap.ContainsKey(neighbour) && floodedMap[neighbour] > floodedMap[pos] + 1)
                {
                    floodedMap[neighbour] = floodedMap[pos] + 1;
                    Flood(floodedMap, portals, neighbour);
                }
            }

        }

        private static (int x, int y) Pos((int x, int y, string name) p) => (p.x, p.y);

        protected override string Part2Code(string data)
        {

            HashSet<(int x, int y)> map = new HashSet<(int x, int y)>();
            int y = 0;
            List<(int x, int y, string name, int d, bool outer)> portalsList = new List<(int x, int y, string name, int d, bool outer)>();
            var lines = data.Lines().ToArray();
            foreach (var line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                    {
                        map.Add((x, y));
                    }

                    if (x < line.Length - 2 && line[x] == '.' && char.IsLetter(line[x + 1]) && char.IsLetter(line[x + 2]))
                    {
                        portalsList.Add((x, y, line[x + 1] + "" + line[x + 2], d: 3, outer: x == 2 || x == line.Length - 3 || y == 2 || y == lines.Length - 3));
                    }
                    if (x < line.Length - 2 && char.IsLetter(line[x]) && char.IsLetter(line[x + 1]) && line[x + 2] == '.')
                    {
                        portalsList.Add((x + 2, y, line[x] + "" + line[x + 1], d: 1, outer: x + 2 == 2 || x + 2 == line.Length - 3 || y == 2 || y == lines.Length - 3));
                    }
                    if (y < lines.Length - 2 && line[x] == '.' && char.IsLetter(lines[y + 1][x]) && char.IsLetter(lines[y + 2][x]))
                    {
                        portalsList.Add((x, y, lines[y + 1][x] + "" + lines[y + 2][x], d: 0, outer: x == 2 || x == line.Length - 3 || y == 2 || y == lines.Length - 3));
                    }
                    if (y < lines.Length - 2 && char.IsLetter(line[x]) && char.IsLetter(lines[y + 1][x]) && lines[y + 2][x] == '.')
                    {
                        portalsList.Add((x, y + 2, line[x] + "" + lines[y + 1][x], d: 2, outer: x == 2 || x == line.Length - 3 || y + 2 == 2 || y + 2 == lines.Length - 3));
                    }
                }
                y++;
            }



            var start = Pos(portalsList.Single(x => x.name == "AA"));
            var end = Pos(portalsList.Single(x => x.name == "ZZ"));
            var startDir = portalsList.Single(x => x.name == "AA").d;
            portalsList = portalsList.Where(x => x.name != "AA" && x.name != "ZZ").ToList();
            var priotityQueue = new Priority_Queue.SimplePriorityQueue<(int x, int y, int z, int d, int cost), int>();
            priotityQueue.Enqueue((start.x, start.y, 0, startDir, 0), 0);
            while (true)
            {
                var current = priotityQueue.Dequeue();
                if ((current.x, current.y) == end && current.z == 0)
                {
                    return current.cost.ToString();
                }
                var normalNeighbours = new List<(int x, int y, int d)> { W((current.x, current.y), current.d), W((current.x, current.y), (current.d - 1 + 4) % 4), W((current.x, current.y), (current.d + 1) % 4) };
                foreach (var n in normalNeighbours)
                {
                    if (map.Contains((n.x, n.y)))
                    {
                        priotityQueue.Enqueue((n.x, n.y, current.z, n.d, current.cost + 1), current.cost + 1);
                    }
                    var ps = ForZ(portalsList, current.z);
                    if (ps.Any(p => p.x == n.x && p.y == n.y))
                    {
                        var enterPortal = ps.Single(p => p.x == n.x && p.y == n.y);
                        var exitPortal = portalsList.Single(p => p.name == enterPortal.name && p.outer == !enterPortal.outer);

                        priotityQueue.Enqueue((exitPortal.x, exitPortal.y, current.z + (enterPortal.outer ? -1 : 1), exitPortal.d, current.cost + 2), current.cost + 2);
                    }
                }
            }
        }
        private static Dictionary<int, List<(int x, int y, string name, int d, bool outer)>> zPortal = new Dictionary<int, List<(int x, int y, string name, int d, bool outer)>>();
        private List<(int x, int y, string name, int d, bool outer)> ForZ(List<(int x, int y, string name, int d, bool outer)> portalsList, int z)
        {
            if (!zPortal.ContainsKey(z))
            {
                zPortal[z] = portalsList
                    .Where(p => (z == 0 && !p.outer) || z > 0)
                    .ToList();
            }
            return zPortal[z];
        }

        private (int x, int y, int d) W((int x, int y) p, int d) => (p.x + D[d].x, p.y + D[d].y, d);

        private static (int x, int y)[] D = new (int x, int y)[] { (0, -1) /*up*/, (1, 0) /*right*/, (0, 1) /*down*/, (-1, 0) /*left*/ };

        private (int x, int y) Pos((int x, int y, string name, int d, bool outer) p) => (p.x, p.y);
    }
}
