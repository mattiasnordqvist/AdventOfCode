using System;
using System.Linq;
using static App.Day5;

namespace App
{
    public class Day5 : Day
    {

        protected override string Part1Code(string data)
        {
            return Compute(data.FromCommaSep().Select(int.Parse).ToArray(), 1).ToString();
        }



        protected override string Part2Code(string data)
        {
            return Compute(data.FromCommaSep().Select(int.Parse).ToArray(), 5).ToString();
        }

        public static int? Compute(int[] memory, int? input)
        {
            int pointer = 0;
            int? output = null;
            var opcodes = new Action<bool, bool, bool>[]
            {
                (_,__,___) => { /*no-op*/},
                (m1,m2,_) => { /*add*/ memory[memory[pointer+3]] = value(m1,pointer+1) + value(m2,pointer+2); pointer+=4; },
                (m1,m2,_) => { /*mul*/memory[memory[pointer+3]] = value(m1,pointer+1) * value(m2,pointer+2); pointer+=4; },
                (_,__,___) => { /*read */ memory[memory[pointer+1]] = input.Value; pointer+=2; },
                (_,__,___) => { /*write*/ output = memory[memory[pointer+1]]; pointer+=2; Console.WriteLine(output); },
                (m1,m2,___) => { /*jump if true*/ pointer = value(m1, pointer+1) == 0 ? pointer+=3 : pointer = value(m2, pointer+2); },
                (m1,m2,___) => { /*jump if false*/ pointer = value(m1, pointer+1) != 0 ? pointer+=3 : pointer = value(m2, pointer+2); },
                (m1,m2,___) => { /*less than*/ memory[memory[pointer+3]] = value(m1, pointer+1) < value(m2, pointer+2) ? 1 : 0; pointer+=4; },
                (m1,m2,___) => { /*equal*/ memory[memory[pointer+3]] = value(m1, pointer+1) == value(m2, pointer+2) ? 1 : 0; pointer+=4; },

            };
            while (memory[pointer] != 99)
            {
                opcodes[memory[pointer] % 100](memory[pointer] / 100 % 10 == 1, memory[pointer] / 1000 % 10 == 1, memory[pointer] / 10000 % 10 == 1);
            }

            return output;

            int value(bool immediate, int value)
            {
                return immediate ? memory[value] : memory[memory[value]];
            }
        }
    }
}
