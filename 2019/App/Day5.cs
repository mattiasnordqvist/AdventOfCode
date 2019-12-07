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

    /* Object-oriented version below. */
    public class IntCodeComputer
    {
        public int[] Memory { get; set; }
        public int? Input { get; set; }

        public int Output { get; internal set; }

        public int? Run(int? input)
        {
            Input = input;
            if (Memory == null || Memory.Length == 0)
            {
                throw new Exception("No program loaded");
            }
            int pointer = 0;
            while (Memory[pointer] != 99)
            {
                var op = ParseOp(ref pointer);
                op.Execute(Memory);
            }
            return Output;
        }

        private IOperator ParseOp(ref int pointer)
        {
            var op = Memory[pointer];
            if (op % 100 == 1)
            {
                var result = new Add(
                    new Parameter(((op / 100) % 10) == 1, Memory[pointer + 1]),
                    new Parameter(((op / 1000) % 10) == 1, Memory[pointer + 2]),
                    Memory[pointer + 3]);
                pointer += 4;
                return result;
            }
            if (op % 100 == 2)
            {
                var result = new Mulitply(
                    new Parameter(((op / 100) % 10) == 1, Memory[pointer + 1]),
                    new Parameter(((op / 1000) % 10) == 1, Memory[pointer + 2]),
                   Memory[pointer + 3]);
                pointer += 4;
                return result;
            }
            if (op % 100 == 3)
            {
                var result = new Input(Memory[pointer + 1], Input.Value);
                pointer += 2;
                return result;
            }
            if (op % 100 == 4)
            {
                var result = new Output(
                    Memory[pointer + 1], new OutputWriter(this));
                pointer += 2;
                return result;
            }
            throw new Exception("Invalid opcode");
        }
    }

    public class Input : IOperator
    {
        private int parameter;
        private int input;

        public Input(int parameter, int input)
        {
            this.parameter = parameter;
            this.input = input;
        }

        public void Execute(int[] memory)
        {
            memory[parameter] = input;
        }
    }

    public class Output : IOperator
    {
        private readonly int position;
        private readonly OutputWriter outputWriter;

        public Output(int position, OutputWriter outputWriter)
        {
            this.position = position;
            this.outputWriter = outputWriter;
        }

        public void Execute(int[] memory)
        {
            var output = (memory[position]);
            outputWriter.SetOutput(output);
            Console.WriteLine(output);
        }
    }

    public class OutputWriter
    {
        private IntCodeComputer intCodeComputer;

        public OutputWriter(IntCodeComputer intCodeComputer)
        {
            this.intCodeComputer = intCodeComputer;
        }

        public void SetOutput(int value)
        {
            intCodeComputer.Output = value;
        }
    }

    public class Add : IOperator
    {
        private readonly Parameter term1;
        private readonly Parameter term2;
        private readonly int output;

        public Add(Parameter term1, Parameter term2, int output)
        {
            this.term1 = term1;
            this.term2 = term2;
            this.output = output;
        }

        public void Execute(int[] memory)
        {
            memory[output] = term1.Value(memory) + term2.Value(memory);
        }
    }

    public class Mulitply : IOperator
    {
        private readonly Parameter factor1;
        private readonly Parameter factor2;
        private readonly int output;

        public Mulitply(Parameter factor1, Parameter factor2, int output)
        {
            this.factor1 = factor1;
            this.factor2 = factor2;
            this.output = output;
        }

        public void Execute(int[] memory)
        {
            memory[output] = factor1.Value(memory) * factor2.Value(memory);
        }
    }

    public class Parameter
    {
        private readonly bool immediate;
        private readonly int value;

        public Parameter(bool immediate, int value)
        {
            this.immediate = immediate;
            this.value = value;
        }

        internal int Value(int[] memory)
        {
            return immediate ? value : memory[value];
        }
    }

    internal interface IOperator
    {
        void Execute(int[] memory);
    }
}
