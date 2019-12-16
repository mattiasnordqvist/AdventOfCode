using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day16 : Day
    {
        protected override string Part1Code(string data)
        {
            
            var input = data.Trim().Select(x => int.Parse(x + "")).ToArray();
            int max = input.Length * 1;
            var output = new int[input.Length * 1];
            for (int it = 0; it < 100; it++)
            {
                for (int o = 0; o < output.Length; o++)
                {
                    output[o] = (int)((Math.Abs(Calc(input, o, max))) % 10L);
                }
                input = output;
            }
            foreach (var a in output)
            {
                Console.Write(a);
                // 89576828
            }
            return "";
        }

        protected override string Part2Code(string data)
        {
            data = "03036732577212944063491565474664";
            var input = data.Trim().Select(x => int.Parse(x + "")).ToArray();
            int max = input.Length * 10000;
            var readFrom = input.Concat(input).ToArray();
            var offset = int.Parse(data.Substring(0, 7));
            var output = new int[input.Count() * 10000];
            for (int it = 0; it < 100; it++)
            {
                for (int o = 0; o < output.Length; o++)
                {
                    output[o] = (int)((Math.Abs(Calc(readFrom,o, max))) % 10L);
                }
                input = output;
            }
            for (int a = offset; a < 8; a++)
            {
                Console.Write(output[a]);
            }
            return "";
        }

        private long Calc(int[] readFrom, int o, int max)
        {
            int from = o;
            int count = o+1;
            long sum = 0;
            int sign = 1;
            while (true)
            {

                int subSum = 0;
                for(int i = 0; i < count; i++)
                {
                    if(from+i >= max) {
                        sum += (subSum * sign);
                        return sum;
                    }
                    subSum += readFrom[(from + i)%readFrom.Length];
                }
                sum += (subSum * sign);
                sign *= -1;
                from += count * 2;
            }
            throw new Exception("unexpected");
        }
    }
}
