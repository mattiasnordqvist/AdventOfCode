using System;
using System.Linq;

namespace App
{
    public class Day2 : Day
    {
        protected override string Part1Code(string data) => Calculate(data, 12, 2).ToString();

        protected override string Part2Code(string data)
        {
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    if(Calculate(data, noun, verb) == 19690720)
                    {
                        return (noun * 100 + verb).ToString();
                    }
                }
            }
            throw new NotImplementedException();

        }

        private int Calculate(string data, int noun, int verb)
        {
            var intCodes = data.FromCommaSep().Select(x => int.Parse(x)).ToArray();
            var pointer = 0;

            intCodes[1] = noun;
            intCodes[2] = verb;
            while (true)
            {
                if (intCodes[pointer] == 99)
                {
                    return intCodes[0];
                }
                else
                {
                    if (intCodes[pointer] == 1)
                    {
                        intCodes[intCodes[pointer + 3]] = intCodes[intCodes[pointer + 1]] + intCodes[intCodes[pointer + 2]];
                    }
                    else if (intCodes[pointer] == 2)
                    {
                        intCodes[intCodes[pointer + 3]] = intCodes[intCodes[pointer + 1]] * intCodes[intCodes[pointer + 2]];
                    }
                }
                pointer += 4;
            }
        }
    }
}
