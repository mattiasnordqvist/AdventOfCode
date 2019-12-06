using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day6 : Day
    {

        protected override string Part1Code(string data)
        {

            var orbits = data.Lines().Select(ParseOrbit).ToDictionary(x => x.Item2);
            return orbits.Values.Sum(x => BuildPathToCOM(x, orbits).Count()).ToString();
        }

        protected override string Part2Code(string data)
        {
            var orbits = data.Lines().Select(ParseOrbit).ToDictionary(x => x.Item2);
            List<string> santasPath = BuildPathToCOM(orbits["SAN"], orbits);
            List<string> yousPath = BuildPathToCOM(orbits["YOU"], orbits);
            int commonOrbitsCount = 0;
            while (true)
            {
                if (santasPath[commonOrbitsCount] != yousPath[commonOrbitsCount])
                {
                    break;
                }
                commonOrbitsCount++;
            }

            return (santasPath.Count() + yousPath.Count() - commonOrbitsCount * 2).ToString();
        }

        private static (string, string) ParseOrbit(string x)
        {
            return (x.Substring(0, x.IndexOf(")")), x.Substring(x.IndexOf(")") + 1));
        }

        private static List<string> BuildPathToCOM((string, string) p, Dictionary<string,(string,string)> orbits)
        {
            // no need to cache. Problem is pretty small.
            var list = new LinkedList<string>();
            list.AddFirst(p.Item1);
            while (p.Item1 != "COM")
            {
                p = orbits[p.Item1];
                list.AddFirst(p.Item1);
            }
            return new List<string>(list);
        }
    }
}
