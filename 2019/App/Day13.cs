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
            var output = new BufferBlock<long>();
            var comp = new IntCodeComputer()
            {
                Memory = program,
                Input = null, // right?
                Output = output,
            };

            var process = Task.Run(() => comp.Compute());
            var tiles = new Dictionary<int, char> { { 0, ' ' }, { 1, '#' }, { 2, '=' }, { 3, '_' }, { 4, 'o' } };
            var state = new Dictionary<(int, int), int>();
            while (true)
            {
                var x = (int)output.Receive();
                var y = (int)output.Receive();
                var tId = (int)output.Receive();
                Console.SetCursorPosition(x, y);
                Console.Write(tiles[tId]);
                state[(x, y)] = tId;
                if (process.IsCompleted && output.Count == 0)
                {
                    return state.Where(x => x.Value == 2).Count().ToString();
                }
            }
        }

        protected override string Part2Code(string data)
        {
            var program = IntCodeComputer.ParseProgram(data);
            program[0] = 2;
            var output = new BufferBlock<long>();
            var state = new ConcurrentDictionary<(int, int), int>();

            var input = new JoyStickInput(state);
            var comp = new IntCodeComputer()
            {
                Memory = program,
                Input = input,
                Output = output,
            };

            var process = Task.Run(() => comp.Compute());
            var tiles = new Dictionary<int, char> { { 0, ' ' }, { 1, '#' }, { 2, '=' }, { 3, '|' }, { 4, 'o' } };
            var score = 0;
            while (true)
            {
                var x = (int)output.Receive();
                var y = (int)output.Receive();
                var tId = (int)output.Receive();
                if (x == -1 && y == 0)
                {
                    Console.SetCursorPosition(0, 20);
                    Console.WriteLine("Score=" + tId);
                    score = tId;
                }
                else
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(tiles[tId]);
                    state[(x, y)] = tId;
                    
                }
                if (process.IsCompleted && output.Count == 0)
                {
                    return score.ToString();
                }
            }
            
        }
    }

    internal class JoyStickInput : IIntCodeComputerInput
    {
        private readonly ConcurrentDictionary<(int, int), int> state;

        public JoyStickInput(ConcurrentDictionary<(int, int), int> state)
        {
            this.state = state;
        }

        public long Receive()
        {

            Console.ReadKey();

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