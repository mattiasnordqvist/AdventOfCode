using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App
{

    public class Day18 : Day
    {
        protected override string Part1Code(string data)
        {
            (int x, int y) start = (40, 40);
            //            start = (8, 4);
            //            data = @"#################
            //#i.G..c...e..H.p#
            //########.########
            //#j.A..b...f..D.o#
            //########@########
            //#k.E..a...g..B.n#
            //########.########
            //#l.F..d...h..C.m#
            //#################";

            //            start = (5, 1);
            //            data = @"#########
            //#bCAc@.a#
            //#########";
            var map = Map.Read(data);
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

            var stack = new Stack<Progress>();
            stack.Push(new Progress(startNode));
            int shortest = int.MaxValue;
            while (stack.Count > 0)
            {
                Progress popped;
                List<(Node, Path)> availableEdges;
                popped = stack.Pop();
                if (popped.Steps >= shortest)
                {
                    continue;
                }
                if (!_cache.ContainsKey((popped.CurrentNode.Key, popped.Keys)))
                {
                    _cache[(popped.CurrentNode.Key, popped.Keys)] = popped.CurrentNode.Edges
                        .Where(x => (popped.Keys | x.Item1.Key) > popped.Keys)
                        .Where(x => (x.Item2.Doors & popped.Keys) == x.Item2.Doors)
                        .Where(x => x.Item1 != startNode)
                        .ToList();
                }

                availableEdges = _cache[(popped.CurrentNode.Key, popped.Keys)];

                foreach (var edge in availableEdges)
                {
                    var newProgress = popped.Walk(edge);
                    if (newProgress.Steps >= shortest)
                    {
                        continue;
                    }
                    if (newProgress.Keys == map.KeySum)
                    {
                        if (newProgress.Steps < shortest)
                        {
                            shortest = newProgress.Steps;
                            Console.WriteLine(shortest + ": ");
                            //Console.WriteLine(newProgress.Breadcrumbs.Distinct().Aggregate("", (a, b) => a + " -> " + b));
                        }
                        continue;
                    }
                    else
                    {
                        stack.Push(newProgress);
                    }
                }
            }
            return "";
        }
        static Random r = new Random();
        private Dictionary<(int, int), List<(Node, Path)>> _cache = new Dictionary<(int, int), List<(Node, Path)>>();

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
            //public List<char> Breadcrumb { get; private set; } = new List<char>();

            //public int KeyCount { get; private set; }
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

        private class Progress : Priority_Queue.FastPriorityQueueNode
        {
            public Progress(Node current)
            {
                CurrentNode = current;
                //Breadcrumbs = new List<char> { '@' };
            }

            private Progress() { }
            //public List<char> Breadcrumbs { get; set; }
            public Node CurrentNode { get; internal set; }
            public int Steps { get; internal set; } = 0;
            public int Keys { get; internal set; } = 0;
            public bool Failed { get; internal set; } = false;

            //public int KeyCount { get; private set; } = 0;

            internal Progress Walk((Node node, Path path) edge)
            {
                var newProgress = new Progress();
                newProgress.Steps = Steps + edge.path.Length;
                newProgress.CurrentNode = edge.node;
                newProgress.Keys = Keys | edge.path.Keys;
                //newProgress.Breadcrumbs = Breadcrumbs.ToList().Concat(edge.path.Breadcrumb).ToList();
                //newProgress.KeyCount = NumberOfSetBits(newProgress.Keys);
                return newProgress;
            }

            int NumberOfSetBits(int i)
            {
                i = i - ((i >> 1) & 0x55555555);
                i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
                return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
            }
        }
    }
}
