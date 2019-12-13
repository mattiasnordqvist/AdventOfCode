using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App
{
    public class Day13 : Day
    {
        protected override string Part1Code(string data)
        {
            var program = IntCodeComputer.ParseProgram(data);
            var comp = new AsyncIntCodeComputer()
            {
                Memory = program
            };

            var process = Task.Run(() => comp.Compute());
            var tiles = new Dictionary<int, char> { { 0, ' ' }, { 1, '#' }, { 2, '=' }, { 3, '_' }, { 4, 'o' } };
            var state = new Dictionary<(int, int), int>();
            while (true)
            {
                var x = (int)comp.Output.Receive();
                var y = (int)comp.Output.Receive();
                var tId = (int)comp.Output.Receive();
                Console.SetCursorPosition(x, y);
                Console.Write(tiles[tId]);
                state[(x, y)] = tId;
                if (process.IsCompleted && comp.Output.Count == 0)
                {
                    return state.Where(x => x.Value == 2).Count().ToString();
                }
            }
        }

        protected override string Part2Code(string data)
        {
            var program = AsyncIntCodeComputer.ParseProgram(data);
            program[0] = 2;
            var state = new Dictionary<(int, int), int>();
            int outputPos = 0;
            int[] output = new int[3];
            int score = 0;
            var tiles = new Dictionary<int, char> { { 0, ' ' }, { 1, '#' }, { 2, '=' }, { 3, '|' }, { 4, 'o' } };
            var input = new JoyStickInput(state);
            var comp = new IntCodeComputer()
            {
                Memory = program,
                ReadInput = () => input.Receive(),
                WriteOutput = (o) =>
                {
                    output[outputPos] = (int)o;
                    outputPos++;
                    if (outputPos == 3)
                    {
                        if (output[0] == -1 && output[1] == 0)
                        {
                            Console.SetCursorPosition(0, 20);
                            Console.WriteLine("Score=" + output[2]);
                            score = output[2];
                        }
                        else
                        {
                            Console.SetCursorPosition(output[0], output[1]);
                            Console.Write(tiles[output[2]]);
                            state[(output[0], output[1])] = output[2];

                        }
                        outputPos = 0;
                    }
                }

            };
          
            comp.Compute();
            return score.ToString();
        }
    }


    internal class JoyStickInput
    {
        private readonly Dictionary<(int, int), int> state;

        public JoyStickInput(Dictionary<(int, int), int> state)
        {
            this.state = state;
        }

        public long Receive()
        {
            
            if (state.Any(x => x.Value == 4) && state.Any(x => x.Value == 3))
            {
                if (state.Single(x => x.Value == 4).Key.Item1 > state.Single(x => x.Value == 3).Key.Item1)
                {
                    return 1;
                }
                else if (state.Single(x => x.Value == 4).Key.Item1 < state.Single(x => x.Value == 3).Key.Item1)
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}