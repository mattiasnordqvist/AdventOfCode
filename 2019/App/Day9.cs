using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App
{
    public class Day9 : Day
    {
        protected override string Part1Code(string data)
        {
            var memory = data.FromCommaSep().Select(long.Parse).ToArray();
            var input = new BufferBlock<long>();
            var output = new BufferBlock<long>();
            input.Post(1); 

            var process = Task.Run(()=> Compute(memory, input, output));

            Task.Run(() => 
            {
                while (true)
                {
                    Console.WriteLine(output.Receive());
                }
            });

            process.Wait();
            return process.Result.ToString();
        }

        protected override string Part2Code(string data)
        {
            var memory = data.FromCommaSep().Select(long.Parse).ToArray();
            var input = new BufferBlock<long>();
            var output = new BufferBlock<long>();
            input.Post(2);

            var process = Task.Run(() => Compute(memory, input, output));

            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(output.Receive());
                }
            });

            process.Wait();
            return process.Result.ToString();
        }

        public static long? Compute(ScaleUpArray<long> memory, BufferBlock<long> input, BufferBlock<long> output)
        {
            long? lastOutput = null;
            long pointer = 0;
            long relativeBase = 0;
            var opcodes = new Action[]
            {
                () => { throw new Exception("no-op"); },
                () => { /*add*/ memory[pos(3)] = value(1) + value(2); pointer+=4; },
                () => { /*mul*/memory[pos(3)] = value(1) * value(2); pointer+=4; },
                () => { /*read */ memory[pos(1)] = input.Receive(); pointer+=2; },
                () => { /*write*/ lastOutput = value(1);  output.Post(lastOutput.Value); pointer+=2; },
                () => { /*jump if true*/ pointer = value(1) == 0 ? pointer+=3 : pointer = value(2); },
                () => { /*jump if false*/ pointer = value(1) != 0 ? pointer+=3 : pointer = value(2); },
                () => { /*less than*/ memory[pos(3)] = value(1) < value(2) ? 1 : 0; pointer+=4; },
                () => { /*equal*/ memory[pos(3)] = value(1) == value(2) ? 1 : 0; pointer+=4; },
                () => { /*relative base offset*/ relativeBase += value(1); pointer+=2; },

            };
            while (memory[pointer] != 99)
            {
                opcodes[memory[pointer] % 100]();
            }

            return lastOutput.Value;

            long pos(long paramPos)
            {
                int paramModifier = ((int)(memory[pointer] / Math.Pow(10, 1 + paramPos) % 10));
                bool position = paramModifier == 0;
                bool immediate = paramModifier == 1;

                return immediate ? pointer + paramPos : position ? memory[pointer + paramPos] : relativeBase + memory[pointer + paramPos];
            }

            long value(long paramPos) => memory[pos(paramPos)];

        }
    }

    public class ScaleUpArray<T>
    {
        private T[] _backingArray;
        public ScaleUpArray(T[] initialValues)
        {
            _backingArray = initialValues.ToArray();
        }

        public static implicit operator ScaleUpArray<T>(T[] data)
        {
            return new ScaleUpArray<T>(data);
        }

        public T this[long i]
        {
            get => i < _backingArray.Length ? _backingArray[i] : (default);
            set
            {
                if (i >= _backingArray.Length)
                {
                    Resize(i*2);
                }
                _backingArray[i] = value;
            }
        }

        private void Resize(long minSize)
        {
            if(_backingArray.Length < minSize)
            {
                var newArray = new T[minSize];
                for(int i = 0; i<_backingArray.Length; i++)
                {
                    newArray[i] = _backingArray[i];
                }
                _backingArray = newArray;
            }
        }
    }
}
