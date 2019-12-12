using System;
using System.Collections.Generic;
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
            // example should give 2772, but it doesn't.
            // I think it is a little strange, looking at the periods of the dimensions of the first moons... 
            // the z dimensions doesn't enter a repeating pattern until step 3. (looking at my output)
            // Why is that? Then how can the initial state (step 0) be the first repeating state?
            // Maybe the z-dimension can enter another repeating pattern, with a longer period, but yielding a smaller common multiple together with the other dimensions?

//            data = @"<x=-1, y=0, z=2>
//<x=2, y=-10, z=-7>
//<x=4, y=-8, z=8>
//<x=3, y=5, z=-1>";
    


            // If we look at the moons one by one, and find their repeating periods, we could find the smallest common multiple of those periods.
            // In the same way, for each moon, we can look at the repeating periods for each dimension, and find smallest common mulitple of all the dimensions.

            (int x, int y, int z)[] moons = Parse(data);
            (int x, int y, int z)[] velocities = moons.Select(x => (0, 0, 0)).ToArray();
            Dictionary<(int m, char d, int p, int v), int> states = new Dictionary<(int m, char d, int p, int v), int>();
            int step = 0;
            Dictionary<(int moon, char d), (int start, int period)> repetitions = new Dictionary<(int moon, char d), (int start, int period)>();
            while (true)
            {
                for (int m = 0; m < moons.Length; m++)
                {
                    if (!repetitions.ContainsKey((m, 'x')))
                    {
                        var xState = (m, 'x', moons[m].x, velocities[m].x);
                        if (states.ContainsKey(xState))
                        {
                            repetitions[(m, 'x')] = (states[xState], step - states[xState]);
                        }
                        else
                        {
                            states[xState] = step;
                        }
                    }

                    if (!repetitions.ContainsKey((m, 'y')))
                    {
                        var yState = (m, 'y', moons[m].y, velocities[m].y);
                        if (states.ContainsKey(yState))
                        {
                            repetitions[(m, 'y')] = (states[yState], step - states[yState]);
                        }
                        else
                        {
                            states[yState] = step;
                        }
                    }

                    if (!repetitions.ContainsKey((m, 'z')))
                    {
                        var zState = (m, 'z', moons[m].z, velocities[m].z);
                        if (states.ContainsKey(zState))
                        {
                            repetitions[(m, 'z')] = (states[zState], step - states[zState]);
                        }
                        else
                        {
                            states[zState] = step;
                        }
                    }
                }
                Console.WriteLine(repetitions.Count());
                if (repetitions.Count == moons.Length * 3)
                {
                    // we found all twelve periods (nr of moons times number of dimensions)
                    break;
                }
                Step();
                step++;
            }
            // so now we can start finding the smallest common multiples of the dimension periods within each moon.
            // if the SCM is higher than the difference between the highest and lowest period start step among these dimensions, 
            // then I think it is safe to say that this repeating pattern starts at this SCM. (and I don't even think we need to test for this.
            long[] eachMoonsPeriod = new long[moons.Length];
            for (int m = 0; m < moons.Length; m++)
            {
                Console.WriteLine($"Moon {m}:");
                Console.WriteLine($"x: s={repetitions[(m, 'x')].start}, p={repetitions[(m, 'x')].period}");
                Console.WriteLine($"y: s={repetitions[(m, 'y')].start}, p={repetitions[(m, 'y')].period}");
                Console.WriteLine($"z: s={repetitions[(m, 'z')].start}, p={repetitions[(m, 'z')].period}");
                var scm = FindSmallestCommonMultiple(repetitions[(m, 'x')].period, repetitions[(m, 'y')].period, repetitions[(m, 'z')].period);
                Console.WriteLine($"SCM: {scm}");
                eachMoonsPeriod[m] = scm;
            }
            
            // Now we can find the SCM of the moons periods;
            return FindSmallestCommonMultiple(eachMoonsPeriod).ToString();
            //sadly, this does not yield the correct result. What am I doing wrong?
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

            long FindSmallestCommonMultiple(params long[] input)
            {
                var calculations = input.ToArray();
                while (true)
                {
                    if (calculations.All(x => x == calculations[0]))
                    {
                        return calculations[0];
                    }
                    long smallestIndex = 0;
                    long smallestValue = calculations[0];
                    for(int i = 1; i < calculations.Length; i++)
                    {
                        if (calculations[i] < smallestValue)
                        {
                            smallestValue = calculations[i];
                            smallestIndex = i;
                        }
                    }
                    calculations[smallestIndex] += input[smallestIndex];
                }
            }
        }
    }
}