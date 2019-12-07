using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App
{
    public class Day7 : Day
    {
        protected override string Part1Code(string data)
        {
            var program = data.FromCommaSep().Select(int.Parse).ToArray();

            var phasesList = GetPhaseCombos(0, 4);
            var bestSignal = 0;
            foreach (var phaseSettings in phasesList)
            {
                var signal = 0;
                for (int i = 0; i < 5; i++)
                {
                    var input = new BufferBlock<int>();
                    input.Post(phaseSettings[i]);
                    input.Post(signal);
                    signal = Compute(program.ToArray(), input, new BufferBlock<int>()).Value;
                }
                if (signal > bestSignal)
                {
                    bestSignal = signal;
                }
            }
            return bestSignal.ToString();
        }

        private List<List<int>> GetPhaseCombos(int min, int max)
        {
            var result = new List<List<int>>() { new List<int> { min } };

            for (int i = min + 1; i <= max; i++)
            {
                var intermedieate = new List<List<int>>();
                foreach (var list in result)
                {
                    intermedieate.AddRange(Vary(list, i));
                }
                result = intermedieate;
            }
            return result;
        }

        private static List<List<int>> Vary(List<int> list, int with)
        {
            var newList = new List<List<int>>();
            for (int i = 0; i < list.Count() + 1; i++)
            {
                newList.Add(list.Take(i).Append(with).Concat(list.Skip(i)).ToList());
            }
            return newList;
        }

        public static int? Compute(int[] memory, BufferBlock<int> input, BufferBlock<int> output)
        {
            int? lastOutput = null;
            int pointer = 0;
            var opcodes = new Action<bool, bool, bool>[]
            {
                (_,__,___) => { /*no-op*/},
                (m1,m2,_) => { /*add*/ memory[memory[pointer+3]] = value(m1,pointer+1) + value(m2,pointer+2); pointer+=4; },
                (m1,m2,_) => { /*mul*/memory[memory[pointer+3]] = value(m1,pointer+1) * value(m2,pointer+2); pointer+=4; },
                (_,__,___) => { /*read */ memory[memory[pointer+1]] = input.Receive(); pointer+=2; },
                (_,__,___) => { /*write*/ lastOutput = memory[memory[pointer+1]];  output.Post(lastOutput.Value); pointer+=2; },
                (m1,m2,___) => { /*jump if true*/ pointer = value(m1, pointer+1) == 0 ? pointer+=3 : pointer = value(m2, pointer+2); },
                (m1,m2,___) => { /*jump if false*/ pointer = value(m1, pointer+1) != 0 ? pointer+=3 : pointer = value(m2, pointer+2); },
                (m1,m2,___) => { /*less than*/ memory[memory[pointer+3]] = value(m1, pointer+1) < value(m2, pointer+2) ? 1 : 0; pointer+=4; },
                (m1,m2,___) => { /*equal*/ memory[memory[pointer+3]] = value(m1, pointer+1) == value(m2, pointer+2) ? 1 : 0; pointer+=4; },

            };
            while (memory[pointer] != 99)
            {
                opcodes[memory[pointer] % 100](memory[pointer] / 100 % 10 == 1, memory[pointer] / 1000 % 10 == 1, memory[pointer] / 10000 % 10 == 1);
            }

            return lastOutput.Value;
            int value(bool immediate, int value)
            {
                return immediate ? memory[value] : memory[memory[value]];
            }

        }


        protected override string Part2Code(string data)
        {
            var program = data.FromCommaSep().Select(int.Parse).ToArray();
            var phasesList = GetPhaseCombos(5, 9);
            var bestSignal = 0;
            foreach (var phaseSettings in phasesList)
            {
                var signal = 0;
                var inputs = new BufferBlock<int>[5];

                for (int i = 0;i<5;i++)
                {
                    inputs[i] = new BufferBlock<int>();
                    inputs[i].Post(phaseSettings[i]);
                }
                inputs[0].Post(0);
                var amplifiers = Enumerable.Range(0, 5)
                    .Select(x => Task.Run(() => Compute(program.ToArray(), inputs[x], inputs[(x + 1) % 5]).Value))
                    .ToList();
                Task.WhenAll(amplifiers).Wait();
                signal = amplifiers.Last().Result;

                if (signal > bestSignal)
                {
                    bestSignal = signal;
                }
            }
            return bestSignal.ToString();
        }
    }
}
