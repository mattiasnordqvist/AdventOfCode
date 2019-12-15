using System;
using System.Collections.Generic;
using System.Threading;

namespace App
{
    public class Day15 : Day
    {
        Dictionary<(int, int), int> map = new Dictionary<(int, int), int>();

        protected override string Part1Code(string data)
        {
            return "";
            var program = IntCodeComputer.ParseProgram(data);
            var computer = new IntCodeComputer() { Memory = program };
            (int x, int y) d = (0, 0);
            (int x, int y) nd = (0, 0);
            (int x, int y)? oxygen = null;
            Console.CursorVisible = false;
            Random r = new Random();
            map[d] = 0;
            computer.ReadInput = () =>
            {
                while (true)
                {
                    var random = r.Next(1, 5);
                    if (random == 1)
                    {
                        nd = (d.x, d.y - 1);
                        return 1;
                    }
                    if (random == 2)
                    {
                        nd = (d.x, d.y + 1);
                        return 2;
                    }
                    if (random == 3)
                    {
                        nd = (d.x - 1, d.y);
                        return 3;
                    }
                    if (random == 4)
                    {
                        nd = (d.x + 1, d.y);
                        return 4;
                    }
                }
            };
            computer.WriteOutput = (o) =>
            {
                if (o == 0)
                {
                    map[nd] = -1;
                }
                else if (o == 1)
                {
                    d = nd;
                    var candidate = Math.Min(Math.Min(Math.Min(mapV((nd.x - 1, nd.y)), mapV((nd.x + 1, nd.y))), mapV((nd.x, nd.y - 1))), mapV((nd.x, nd.y + 1))) + 1;
                    if (!map.ContainsKey(nd))
                    {
                        map[nd] = candidate;
                    }
                    else
                    {
                        map[nd] = Math.Min(candidate, map[nd]);
                    }
                }
                else if (o == 2)
                {
                    oxygen = nd;
                    d = nd;
                    var candidate = Math.Min(Math.Min(Math.Min(mapV((nd.x - 1, nd.y)), mapV((nd.x + 1, nd.y))), mapV((nd.x, nd.y - 1))), mapV((nd.x, nd.y + 1))) + 1;
                    if (candidate > 0)
                    {
                        if (!map.ContainsKey(nd))
                        {
                            map[nd] = candidate;
                        }
                        else
                        {
                            map[nd] = Math.Min(candidate, map[nd]);
                        }
                        int shortest = map[nd];
                        Console.WriteLine(shortest);
                    }
                }


            };
            computer.Compute();
            return "";
        }

        private int mapV((int x, int y) p)
        {
            if (map.ContainsKey(p) && map[p] >= 0)
            {
                return map[p];
            }
            else
            {
                return int.MaxValue;
            }
        }

        protected override string Part2Code(string data)
        {
            var program = IntCodeComputer.ParseProgram(data);
            var computer = new IntCodeComputer() { Memory = program };
            (int x, int y) lastd = (0, 0);
            (int x, int y) d = (0, 0);
            (int x, int y) nd = (0, 0);
            (int x, int y)? oxygen = null;
            Console.CursorVisible = false;
            Random r = new Random();
            map[d] = 0;
            computer.ReadInput = () =>
            {
                while (true)
                {
                    var random = r.Next(1, 5);
                    if (random == 1)
                    {
                        nd = (d.x, d.y - 1);
                        return 1;
                    }
                    if (random == 2)
                    {
                        nd = (d.x, d.y + 1);
                        return 2;
                    }
                    if (random == 3)
                    {
                        nd = (d.x - 1, d.y);
                        return 3;
                    }
                    if (random == 4)
                    {
                        nd = (d.x + 1, d.y);
                        return 4;
                    }
                }
            };
            int max = 0;
            computer.WriteOutput = (o) =>
            {
                if (o == 0)
                {
                    map[nd] = -1;
                }
                else if (o == 1)
                {
                    d = nd;
                    var candidate = Math.Min(Math.Min(Math.Min(mapV((nd.x - 1, nd.y)), mapV((nd.x + 1, nd.y))), mapV((nd.x, nd.y - 1))), mapV((nd.x, nd.y + 1))) + 1;
                    if (!map.ContainsKey(nd))
                    {
                        map[nd] = candidate;
                    }
                    else
                    {
                        map[nd] = Math.Min(candidate, map[nd]);
                    }
                    if (oxygen.HasValue)
                    {
                        if (map[nd] > max)
                        {
                            max = map[nd];
                            Console.WriteLine(max);
                        }
                    }
                }
                else if (o == 2)
                {
                    if (oxygen == null)
                    {
                        oxygen = nd;
                        map = new Dictionary<(int, int), int>();
                        map[oxygen.Value] = 0;
                    }
                    d = nd;
                }


            };
            computer.Compute();
            return "";
        }
    }
}