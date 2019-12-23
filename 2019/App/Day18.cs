using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App
{

    public class Day18 : Day
    {
        static int shortest = int.MaxValue;
        static Dictionary<int, (int K, (int K, int KS, int DS, int L)[] E)> m;
        static int allKeys = 0;
        protected override string Part1Code(string data)
        {
            (int x, int y) start;

            //            data = @"#########
            //#b.A.@.a#
            //#########";
            data = @"########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################";
            //            data = @"########################
            //#...............b.C.D.f#
            //#.######################
            //#.....@.a.B.c.d.A.e.F.g#
            //########################";

            //            data = @"#################
            //#i.G..c...e..H.p#
            //########.########
            //#j.A..b...f..D.o#
            //########@########
            //#k.E..a...g..B.n#
            //########.########
            //#l.F..d...h..C.m#
            //#################";


//            data = @"########################
//#@..............ac.GI.b#
//###d#e#f################
//###A#B#C################
//###g#h#i################
//########################";
            var map = Map.Read(data);
            start = map.Start;
            List<Node> nodes = map.K.Select(x => new Node { Key = x.Key }).ToList();
            var paths = new List<Path>();
            foreach (var key in map.K)
            {
                paths = new List<Path>();
                Flood(map, key.Key, key.Value, null, new Path(), paths);
                nodes.Single(x => x.Key == key.Key).Edges = paths.Select(x => (nodes.Single(n => n.Key == x.LastKey), path: x)).ToList();
            }

            paths = new List<Path>();
            Flood(map, 0, start, null, new Path(), paths);
            var startNode = new Node();
            startNode.Edges = paths.Select(x => (nodes.Single(n => n.Key == x.LastKey), x)).ToList();
            nodes.Add(startNode);

            m = nodes.Select(x => (K: x.Key, E: x.Edges.Select(e => ( K: e.Item1.Key, KS: e.Item2.Keys, DS: e.Item2.Doors, L: e.Item2.Length )).ToArray())).ToArray().ToDictionary(x => x.K, x=> x);
            allKeys = nodes.Aggregate(0, (a, b) => a | b.Key);
            Examine((0, 0), 0);
            return "";
        }

        private void Examine((int K, int KS) N, int L)
        {
            if (L < shortest && N.KS == allKeys)
            {
                shortest = L;
                Console.WriteLine(shortest);
                return;
            }

            if (_shortestPath.ContainsKey(N))
            {
                if(L < _shortestPath[N])
                {
                    _shortestPath[N] = L;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _shortestPath[N] = L;
            }

            var traversableEdges = m[N.K].E
                .Where(x => (N.KS | x.K) > N.KS)
                .Where(x => (x.DS & N.KS) == x.DS)
                .Where(x => x.K != 0)
                .ToList();

            foreach (var edge in traversableEdges)
            {
                Examine((edge.K, N.KS | edge.KS), L + edge.L);
            }

        }

        static Random r = new Random();
        private Dictionary<(int K, int KS), int> _shortestPath = new Dictionary<(int K, int KS), int>();

        private void Flood(Map map, int ignoreKey, (int x, int y) position, (int x, int y)? ignore, Path path, List<Path> paths, HashSet<(int x, int y)> visited = null)
        {
            visited = visited ?? new HashSet<(int x, int y)>();
            visited.Add(position);
            var foundKey = map.KR.ContainsKey(position) ? (map.KR[position] != ignoreKey ? map.KR[position] : 0) : 0;
            var foundDoor = map.DR.ContainsKey(position) ? (map.DR[position] != ignoreKey ? map.DR[position] : 0) : 0;
            path = path.With(foundKey, foundDoor);
            if (foundKey != 0)
            {
                paths.Add(path.With());
            }
            var neighbours = Map.GetNeighBours(map.N, position, ignore);
            path.Length++;
            foreach (var neighbour in neighbours.Where(x => !visited.Contains(x)))
            {
                Flood(map, ignoreKey, neighbour, position, path, paths, visited);
            }
        }

        public class Node
        {
            public int Key { get; set; }
            public List<(Node, Path)> Edges { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Node ? Key.Equals(((Node)obj).Key) : base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Key);
            }
        }

        public class Path
        {
            public int Doors { get; private set; } = 0;
            public int Keys { get; private set; } = 0;
            public int LastKey { get; private set; }
            
            public int Length = 0;
            public Path With(int key = 0, int door = 0)
            {
                //int c = 0;

                //if (key != 0)
                //{
                //    var a = key;
                //    while (true)
                //    {
                //        if (a == 1)
                //        {
                //            break;
                //        }
                //        else
                //        {
                //            c++;
                //            a = a >> 1;
                //        }

                //    }
                //}
                return new Path
                {
                    Length = Length,
                    Doors = Doors | door,
                    Keys = Keys | key,
                    LastKey = key == 0 ? LastKey : key,
                    //KeyCount = KeyCount + (key == 0 ? 1 : 0),
                    //Breadcrumb = key == 0 ? Breadcrumb.ToList() : Breadcrumb.Append((char)(c + 97)).ToList(),
                };
            }
        }

        protected override string Part2Code(string data)
        {
            return "";
        }

        public class Map
        {
            public HashSet<(int x, int y)> N = new HashSet<(int, int)>();
            public Dictionary<int, (int x, int y)> K = new Dictionary<int, (int x, int y)>();
            public Dictionary<int, (int x, int y)> D = new Dictionary<int, (int x, int y)>();
            public Dictionary<(int x, int y), int> KR = new Dictionary<(int x, int y), int>();
            public Dictionary<(int x, int y), int> DR = new Dictionary<(int x, int y), int>();

            public int KeySum { get; private set; }
            public (int x, int y) Start { get; private set; }

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
                    Console.Write(k.Key);
                }
                foreach (var d in D)
                {
                    Console.SetCursorPosition(d.Value.x, d.Value.y);
                    Console.Write(d.Key);
                }

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
                int keySum = 0;
                foreach (var line in data.Lines())
                {
                    foreach (var c in line)
                    {
                        if (c == '.' || c == '@')
                        {
                            map.N.Add((x, y));
                            if(c == '@')
                            {
                                map.Start = ((x, y));
                            }
                        }
                        if (char.IsLetter(c))
                        {
                            map.N.Add((x, y));
                            if (char.IsUpper(c))
                            {
                                int door = 1 << (c - 65);
                                map.D[door] = (x, y);
                                map.DR[(x, y)] = door;
                            }
                            if (char.IsLower(c))
                            {
                                int key = 1 << (c - 97);
                                keySum += key;
                                map.K[key] = (x, y);
                                map.KR[(x, y)] = key;
                            }
                        }
                        x++;
                    }
                    y++;
                    x = 0;
                }
                map.KeySum = keySum;
                return map;
            }
        }



        static int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}
