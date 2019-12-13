using System;
using System.Linq;

namespace App
{
    public class IntCodeComputer
    {
        public static long[] ParseProgram(string program)
        {
            return program.FromCommaSep().Select(long.Parse).ToArray();
        }
        public ScaleUpArray<long> Memory { get; set; }
        public Func<long> ReadInput { get; set; }
        public Action<long> WriteOutput { get; set; }
        public long? Compute()
        {
            long? lastOutput = null;
            long pointer = 0;
            long relativeBase = 0;
            var opcodes = new Action[]
            {
                () => { throw new Exception("no-op"); },
                () => { /*add*/ Memory[pos(3)] = value(1) + value(2); pointer+=4; },
                () => { /*mul*/Memory[pos(3)] = value(1) * value(2); pointer+=4; },
                () => { /*read */ Memory[pos(1)] = ReadInput(); pointer+=2; },
                () => { /*write*/ lastOutput = value(1);  WriteOutput(lastOutput.Value); pointer+=2; },
                () => { /*jump if true*/ pointer = value(1) == 0 ? pointer+=3 : pointer = value(2); },
                () => { /*jump if false*/ pointer = value(1) != 0 ? pointer+=3 : pointer = value(2); },
                () => { /*less than*/ Memory[pos(3)] = value(1) < value(2) ? 1 : 0; pointer+=4; },
                () => { /*equal*/ Memory[pos(3)] = value(1) == value(2) ? 1 : 0; pointer+=4; },
                () => { /*relative base offset*/ relativeBase += value(1); pointer+=2; },
            };

            while (Memory[pointer] != 99)
            {
                opcodes[Memory[pointer] % 100]();
            }

            return lastOutput;

            long pos(long paramPos)
            {
                int paramModifier = ((int)(Memory[pointer] / Math.Pow(10, 1 + paramPos) % 10));
                bool position = paramModifier == 0;
                bool immediate = paramModifier == 1;

                return immediate ? pointer + paramPos : position ? Memory[pointer + paramPos] : relativeBase + Memory[pointer + paramPos];
            }

            long value(long paramPos) => Memory[pos(paramPos)];

        }
    }
}