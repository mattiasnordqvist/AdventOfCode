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
            (int x, int y, int z)[] pos = Parse(data);
            (int x, int y, int z)[] initPos = Parse(data);
            (int x, int y, int z)[] vel = pos.Select(x => (0, 0, 0)).ToArray();
            int step = 0;
            int xRepeat = -1, yRepeat = -1, zRepeat = -1;
            while (true)
            {
                Step();
                step++;
                if (xRepeat == -1 && Enumerable.Range(0, 4).All(m => pos[m].x == initPos[m].x && vel[m].x == 0))
                        xRepeat = step;
                if (yRepeat == -1 && Enumerable.Range(0, 4).All(m => pos[m].y == initPos[m].y && vel[m].y == 0))
                        yRepeat = step;
                if (zRepeat == -1 && Enumerable.Range(0, 4).All(m => pos[m].z == initPos[m].z && vel[m].z == 0))
                        zRepeat = step;
                if (xRepeat != -1 && yRepeat != -1 && zRepeat != -1)
                    return LCM(xRepeat, yRepeat, zRepeat).ToString();
            }

            void Step()
            {
                /* gravity */
                for (int i = 0; i < pos.Length; i++)
                {
                    for (int j = i + 1; j < pos.Length; j++)
                    {
                        vel[i].x += Math.Sign(pos[j].x - pos[i].x);
                        vel[j].x += Math.Sign(pos[i].x - pos[j].x);
                        vel[i].y += Math.Sign(pos[j].y - pos[i].y);
                        vel[j].y += Math.Sign(pos[i].y - pos[j].y);
                        vel[i].z += Math.Sign(pos[j].z - pos[i].z);
                        vel[j].z += Math.Sign(pos[i].z - pos[j].z);
                    }
                }

                /* velocity */
                for (int i = 0; i < pos.Length; i++)
                {
                    pos[i].x += vel[i].x;
                    pos[i].y += vel[i].y;
                    pos[i].z += vel[i].z;
                }
            }
        }

        static long LCM(params long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}