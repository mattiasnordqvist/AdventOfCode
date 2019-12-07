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
            var opcodes = new Action[]
            {
                () => { throw new Exception("no-op"); },
                () => { /*add*/ memory[memory[pointer+3]] = value(1) + value(2); pointer+=4; },
                () => { /*mul*/memory[memory[pointer+3]] = value(1) * value(2); pointer+=4; },
                () => { /*read */ memory[memory[pointer+1]] = input.Receive(); pointer+=2; },
                () => { /*write*/ lastOutput = memory[memory[pointer+1]];  output.Post(lastOutput.Value); pointer+=2; },
                () => { /*jump if true*/ pointer = value(1) == 0 ? pointer+=3 : pointer = value(2); },
                () => { /*jump if false*/ pointer = value(1) != 0 ? pointer+=3 : pointer = value(2); },
                () => { /*less than*/ memory[memory[pointer+3]] = value(1) < value(2) ? 1 : 0; pointer+=4; },
                () => { /*equal*/ memory[memory[pointer+3]] = value(1) == value(2) ? 1 : 0; pointer+=4; },

            };
            while (memory[pointer] != 99)
            {
                opcodes[memory[pointer] % 100]();
            }

            return lastOutput.Value;

            int value(int paramPos)
            {
                bool immediate = ((int)(memory[pointer] / Math.Pow(10, 1 + paramPos) % 10)) == 1;
                return immediate ? memory[pointer+paramPos] : memory[memory[pointer + paramPos]];
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
