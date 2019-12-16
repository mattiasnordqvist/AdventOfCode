using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day16 : Day
    {
        private static int[] input;
        private static int times;

        private static Dictionary<(int, int), long> _cache = new Dictionary<(int, int), long>();


        protected override string Part1Code(string data)
        {
            times = 1;
            input = data.Trim().Select(x => int.Parse(x + "")).ToArray();
            var returnValue = "";
            for (int i = 0; i < 8; i++)
            {
                returnValue+=GetValue(100,i);
                // 89576828
            }
            return returnValue ;
        }

        protected override string Part2Code(string data)
        {
            var offset = int.Parse(data.Substring(0, 7));
            var input = Repeat(data.Trim().Select(x => int.Parse(x + "")).ToArray(),10000).ToArray();
            for (int i = 0; i < 100; i++)
            {
                var output = new int[input.Length];
                var sum = 0;
                for(int p = input.Length - 1; p > input.Length/2; p--)
                {
                    sum += input[p];
                    output[p] = sum%10;
                }
                input = output;
            }
            var returnValue = "";
            for (int i = offset; i < offset+8; i++)
            {
                returnValue += input[i];
            }
            return returnValue;
        }

        private IEnumerable<int> Repeat(int[] v1, int v2)
        {
            for(int i = 0; i < v2; i++)
            {
                for(int j = 0; j < v1.Length; j++)
                {
                    yield return v1[j];
                }
            }
        }

        public static long GetValue(int phase, int offset)
        {
            if (!_cache.ContainsKey((phase, offset)))
            {
                int from = offset;
                int count = offset + 1;
                int max = input.Length * times;
                long sum = 0;
                int i = from;
                int sign = 1;
                int j = 0;
                while (i < max)
                {
                    if (phase > 1)
                    {
                        sum += GetValue(phase - 1, i) * sign;
                    }
                    else
                    {
                        sum += input[i % input.Length] * sign;
                    }
                    j++;
                    if (j == count)
                    {
                        j = 0;
                        sign *= -1;
                        i += count + 1;
                    }
                    else
                    {
                        i++;
                    }
                }
                _cache[(phase, offset)] = Math.Abs(sum)%10L;
            }
            return _cache[(phase, offset)];
        }
    }
}
