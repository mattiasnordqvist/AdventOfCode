using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day16 : Day
    {
        private static int[] input;
        private static int times;

        protected override string Part1Code(string data)
        {
            times = 1;
            input = data.Trim().Select(x => int.Parse(x + "")).ToArray();
            for (int i = 0; i < 8; i++)
            {
                Console.Write(GetValue(100,i));
                // 89576828
            }
            return "";
        }

        public static int[] ApplyPhase(int[] input, int inputrepeats)
        {
            int max = input.Length * inputrepeats;
            var output = new int[max];
            for (int outputPos = 0; outputPos < output.Length; outputPos++)
            {
                output[outputPos] = (int)(Math.Abs(Calc(input, outputPos, max)) % 10L);
            }
            return output;
        }

        protected override string Part2Code(string data)
        {
            data = "03036732577212944063491565474664";
            var offset = int.Parse(data.Substring(0, 7));
            times = 10000;
            input = data.Trim().Select(x => int.Parse(x + "")).ToArray();
            for (int i = offset; i < offset+8; i++)
            {
                Console.Write(GetValue(100, i));
                // 89576828
            }
            return "";
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
        private static Dictionary<(int, int), long> _cache = new Dictionary<(int, int), long>();
        public static long Calc(int[] input, int patternRepetitionIndex, int max)
        {
            var inputSum = input.Sum();
            int from = patternRepetitionIndex;
            int count = patternRepetitionIndex + 1;
            long sum = 0;
            int sign = 1;
            while (true)
            {
                int subSum = 0;
                int j = from;
                while (count > input.Length && j + input.Length < count)
                {
                    subSum += inputSum;
                    j += input.Length;
                }
                for (int i = j; i < from + count; i++)
                {
                    if (i >= max)
                    {
                        sum += subSum * sign;
                        return sum;
                    }

                    else
                    {
                        subSum += input[i % input.Length];
                    }
                }
                sum += subSum * sign;
                sign *= -1;
                from += count * 2;
            }

            throw new Exception("unexpected");
        }
    }
}
