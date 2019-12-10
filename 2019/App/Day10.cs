using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day10 : Day
    {
        private List<(int x, int y)> asteroids = new List<(int x, int y)>();
        private (int nr, (int x, int y) monitor) part1answer;
        protected override string Part1Code(string data)
        {
            var y = 0;
            foreach (var line in data.Lines())
            {
                var x = 0;
                foreach (var c in line)
                {
                    if (c == '#')
                    {
                        asteroids.Add((x, y));
                    }
                    x++;
                }
                y++;
            }
            var calc = asteroids
                .Select(candidate => new
                {
                    candidate,
                    visibleasteroids = asteroids
                    .Where(o => o != candidate)
                    .Select(x => Angle(candidate, x))
                    .Distinct()
                    .Count()
                })
                .OrderByDescending(x => x.visibleasteroids).ToList();
            part1answer = (calc.First().visibleasteroids, calc.First().candidate);

            return part1answer.nr + " (" + part1answer.monitor.x + "," + part1answer.monitor.y + ")";
        }

        public static double Angle((int x, int y) candidate, (int x, int y) other)
        {
            float xDiff = other.x - candidate.x;
            float yDiff = other.y - candidate.y;
            
            var angle = Math.Round(Math.Atan2(-yDiff, xDiff) * 180.0 / Math.PI, 3);
            //normalize?
            return angle < 0 ? angle + 360 : angle;
        }
        public static double Distance((int x, int y) candidate, (int x, int y) other)
        {
            return Math.Sqrt(Math.Pow(other.x - candidate.x, 2) + Math.Pow(other.y - candidate.y, 2));
        }

        protected override string Part2Code(string data)
        {
            var laser = part1answer.monitor;
            asteroids = asteroids.Where(a => !(a.x == laser.x && a.y == laser.y)).ToList();
            var groups = asteroids
                .GroupBy(x => Angle(laser, x))
                .Select(x => new { Angle = x.Key, Asteroids = x.OrderBy(a => Distance(laser, a)).ToList() })
                .OrderByDescending(x => x.Angle)
                .ToList();

            double currentAngle = 90.00001d;
            int i= 0;
            while(groups.Any())
            {
                i++;
                if (!groups.Any(x => x.Angle < currentAngle))
                {
                    currentAngle = 360;
                }
                var group = groups.First(x => x.Angle < currentAngle);
                currentAngle = group.Angle;
                if (i == 200)
                {
                    return (group.Asteroids[0].x * 100 + group.Asteroids[0].y).ToString();
                }

                group.Asteroids.RemoveAt(0);
                if (group.Asteroids.Count == 0)
                {
                    groups.Remove(group);
                }
            }
            throw new Exception("unexpected");
        }
    }
}