using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace App
{
    public class Day12 : Day
    {
        protected override string Part1Code(string data)
        {
            (int x, int y, int z)[] moons = Parse(data);
            (int x, int y, int z)[] velocities = moons.Select(x => (0, 0, 0)).ToArray();

            for (int i = 0; i < 1000; i++)
            {
                Step();
            }

            var energy = 0;
            for (int i = 0; i < moons.Length; i++)
            {
                energy += ((Math.Abs(moons[i].x) + Math.Abs(moons[i].y) + Math.Abs(moons[i].z)) *
                    (Math.Abs(velocities[i].x) + Math.Abs(velocities[i].y) + Math.Abs(velocities[i].z)));
            }
            return energy.ToString();

            void Step()
            {
                /* gravity */
                for (int i = 0; i < moons.Length; i++)
                {
                    for (int j = i + 1; j < moons.Length; j++)
                    {
                        velocities[i].x += Math.Sign(moons[j].x - moons[i].x);
                        velocities[j].x += Math.Sign(moons[i].x - moons[j].x);
                        velocities[i].y += Math.Sign(moons[j].y - moons[i].y);
                        velocities[j].y += Math.Sign(moons[i].y - moons[j].y);
                        velocities[i].z += Math.Sign(moons[j].z - moons[i].z);
                        velocities[j].z += Math.Sign(moons[i].z - moons[j].z);
                    }
                }

                /* velocity */
                for (int i = 0; i < moons.Length; i++)
                {
                    moons[i].x += velocities[i].x;
                    moons[i].y += velocities[i].y;
                    moons[i].z += velocities[i].z;
                }
            }
        }

        private (int x, int y, int z)[] Parse(string data) => data.Lines()
                .Select(line => (x: int.Parse(Regex.Match(line, @"x=(-?[0-9]*)").Groups[1].Value),
                y: int.Parse(Regex.Match(line, @"y=(-?[0-9]*)").Groups[1].Value),
                z: int.Parse(Regex.Match(line, @"z=(-?[0-9]*)").Groups[1].Value))).ToArray();

        protected override string Part2Code(string data)
        {
            return "";
        }
    }
}