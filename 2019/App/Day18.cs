using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace App
{
    public class Day18 : Day
    {
        protected override string Part1Code(string data)
        {
            (int x, int y) start = (40, 40);
//            start = (1, 1);
//            data = @"########################
//#@..............ac.GI.b#
//###d#e#f################
//###A#B#C################
//###g#h#i################
//########################";
            var map = Map.Read(data);
            var originalMap = map.Clone();
            map.Reduce(start);
            var stack = new Stack<(Map map, (int x, int y) pos, int l, HashSet<char> keys)>();
            stack.Push((map, (start.x, start.y), 0, new HashSet<char>()));
            int shortest = int.MaxValue;
            while (stack.Count > 0)
            {
                var popped = stack.Pop();
                if (popped.l >= shortest)
                {
                    continue;
                }
                var m = popped.map;
                foreach(var k in popped.keys)
                {
                    m.ProvideKey(k);
                }
                var foundKeys = m.Flood(popped.pos);
                if (foundKeys.Count == 0)
                {
                    if (popped.l < shortest)
                    {
                        shortest = popped.l;
                        Console.WriteLine(shortest);
                    }
                }
                else
                {
                    foreach (var keyFound in foundKeys)
                    {
                        if (keyFound.Value.l + popped.l >= shortest)
                        {
                            continue;
                        }
                        var newMap = map.Clone();
                        newMap.ProvideKey(keyFound.Key);
                        newMap.Reduce((keyFound.Value.x, keyFound.Value.y));
                        var keys = new HashSet<char>(popped.keys);
                        keys.Add(keyFound.Key);
                        stack.Push((originalMap.Clone(), (keyFound.Value.x, keyFound.Value.y), keyFound.Value.l + popped.l, keys));
                    }
                }
            }
            return shortest.ToString();
        }

        protected override string Part2Code(string data)
        {
            return "";
        }

        public class Map
        {
            public HashSet<(int x, int y)> N = new HashSet<(int, int)>();
            public Dictionary<char, (int x, int y)> K = new Dictionary<char, (int x, int y)>();
            public Dictionary<char, (int x, int y)> D = new Dictionary<char, (int x, int y)>();
            public Dictionary<(int x, int y), char> KR = new Dictionary<(int x, int y), char>();
            public Dictionary<(int x, int y), char> DR = new Dictionary<(int x, int y), char>();

            public Dictionary<char, (int x, int y, int l)> Flood((int x, int y) pos)
            {
                var flood = new Dictionary<(int x, int y), int>();
                var foundKeys = new Dictionary<char, (int x, int y, int l)>();
                Stack<((int x, int y) pos, (int x, int y)? ignore)> stack = new Stack<((int x, int y) pos, (int x, int y)? ignore)>();
                flood[pos] = 0;
                stack.Push((pos, null));
                while (stack.Count > 0)
                {
                    var popped = stack.Pop();
                    if (KR.ContainsKey(popped.pos))
                    {
                        if (!foundKeys.ContainsKey(KR[popped.pos]) || foundKeys[KR[popped.pos]].l > flood[popped.pos])
                        {
                            foundKeys[KR[popped.pos]] = (popped.pos.x, popped.pos.y, flood[popped.pos]);
                        }
                        continue;
                    }
                    var neighBours = GetNeighBours(N, popped.pos, popped.ignore);
                    foreach (var neighbour in neighBours)
                    {

                        if (!flood.ContainsKey(neighbour) || flood[popped.pos] + 1 < flood[neighbour])
                        {
                            flood[neighbour] = flood[popped.pos] + 1;
                            stack.Push((neighbour, popped.pos));
                        }
                    }
                }
                return foundKeys;
            }

            public void ProvideKey(char k)
            {
                if (K.ContainsKey(k))
                {
                    KR.Remove(K[k]);
                    K.Remove(k);
                }
                if (D.ContainsKey(k))
                {
                    N.Add(D[k]);
                    DR.Remove(D[k]);
                    D.Remove(k);
                }

            }

            public HashSet<(int x, int y)> Reduce((int x, int y) fromPos)
            {
                var visits = new HashSet<(int x, int y)>() { (fromPos.x, fromPos.y) };
                Reduce(fromPos, null, visits);
                return visits;
            }

            public void Print()
            {
                Console.Clear();
                foreach (var n in N)
                {
                    Console.SetCursorPosition(n.x, n.y);
                    Console.Write('.');
                }
                foreach (var k in K)
                {
                    Console.SetCursorPosition(k.Value.x, k.Value.y);
                    Console.Write(char.ToLower(k.Key));
                }
                foreach (var d in D)
                {
                    Console.SetCursorPosition(d.Value.x, d.Value.y);
                    Console.Write(d.Key);
                }

            }

            private bool Reduce((int x, int y) fromPos, (int x, int y)? ignore, HashSet<(int x, int y)> visits)
            {
                visits.Add(fromPos);
                var neighbours = GetNeighBours(N, fromPos, ignore)
                    .Where(n => !DR.ContainsKey(n))
                    .ToList();
                if (neighbours.Count() > 0 && neighbours.All(x => visits.Contains(x)))
                {
                    return true;
                }
                neighbours = neighbours
                    .Where(x => !visits.Contains(x))
                    .ToList();
                var foundSomething = KR.ContainsKey(fromPos);
                foreach (var n in neighbours)
                {
                    if (Reduce(n, fromPos, visits))
                    {
                        foundSomething = true;
                    }
                    else
                    {
                        N.Remove(n);
                    }
                }
                return foundSomething;
            }

            public Map Clone()
            {
                return new Map
                {
                    N = N.ToHashSet(),
                    K = K.ToDictionary(x => x.Key, x => x.Value),
                    D = D.ToDictionary(x => x.Key, x => x.Value),
                    KR = KR.ToDictionary(x => x.Key, x => x.Value),
                    DR = DR.ToDictionary(x => x.Key, x => x.Value),
                };
            }

            public static IEnumerable<(int x, int y)> GetNeighBours(HashSet<(int x, int y)> n, (int x, int y) p, (int x, int y)? i)
            {
                var l = (p.x - 1, p.y);
                var r = (p.x + 1, p.y);
                var u = (p.x, p.y - 1);
                var d = (p.x, p.y + 1);
                if (!i.HasValue)
                {
                    if (n.Contains(l)) yield return l;
                    if (n.Contains(r)) yield return r;
                    if (n.Contains(u)) yield return u;
                    if (n.Contains(d)) yield return d;
                }
                else
                {
                    if (i != l && n.Contains(l)) yield return l;
                    if (i != r && n.Contains(r)) yield return r;
                    if (i != u && n.Contains(u)) yield return u;
                    if (i != d && n.Contains(d)) yield return d;
                }

            }

            public static Map Read(string data)
            {
                Map map = new Map();
                int x = 0;
                int y = 0;
                foreach (var line in data.Lines())
                {
                    foreach (var c in line)
                    {
                        if (c == '.' || c == '@')
                        {
                            map.N.Add((x, y));
                        }
                        if (char.IsLetter(c))
                        {
                            if (char.IsUpper(c))
                            {
                                map.D[c] = (x, y);
                                map.DR[(x, y)] = c;
                            }
                            if (char.IsLower(c))
                            {
                                map.N.Add((x, y));
                                map.K[char.ToUpper(c)] = (x, y);
                                map.KR[(x, y)] = char.ToUpper(c);
                            }
                        }
                        x++;
                    }
                    y++;
                    x = 0;
                }
                return map;
            }
        }
    }
}
