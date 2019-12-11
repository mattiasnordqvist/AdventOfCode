using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App
{
    public class Day11 : Day
    {
        protected override string Part1Code(string data)
        {
            Dictionary<(int x, int y), int> panels = new Dictionary<(int x, int y), int>();
            var robotpos = (x: 0, y: 0);
            var dirs = new (int x, int y)[] { (x: 0, y: 1), (x: 1, y: 0), (x: 0, y: -1), (x: -1, y: 0) };
            var robotdir = 0;
            var memory = data.FromCommaSep().Select(long.Parse).ToArray();

            var input = new BufferBlock<long>();
            input.Post(0);
            var output = new BufferBlock<long>();
            var process = Task.Run(() => Compute(memory, input, output));
            var panelsPainted = 0;
            while (true)
            {
                if (!panels.ContainsKey(robotpos))
                {
                    panelsPainted++;
                }
                panels[robotpos] = (int)output.Receive();
                var turn = (int)output.Receive();
                if (turn == 1)
                {
                    robotdir = (robotdir + 1) % 4;
                }
                else if (turn == 0)
                {
                    robotdir = (robotdir - 1 + 4) % 4;
                }
                else
                {
                    throw new Exception("unexpected");
                }
                robotpos = (x: robotpos.x + dirs[robotdir].x, y: robotpos.y + dirs[robotdir].y);
                if (process.IsCompleted)
                {
                    break;
                }
                input.Post(panels.ContainsKey(robotpos) ? panels[robotpos] : 0);
            }
            return panelsPainted.ToString();
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
        protected override string Part2Code(string data)
        {
            Dictionary<(int x, int y), int> panels = new Dictionary<(int x, int y), int>();
            var robotpos = (x: 0, y: 0);
            var dirs = new (int x, int y)[] { (x: 0, y: 1), (x: 1, y: 0), (x: 0, y: -1), (x: -1, y: 0) };
            var robotdir = 0;
            var memory = data.FromCommaSep().Select(long.Parse).ToArray();

            var input = new BufferBlock<long>();
            input.Post(1);
            var output = new BufferBlock<long>();
            var process = Task.Run(() => Compute(memory, input, output));
            var panelsPainted = 0;
            while (true)
            {
                if (!panels.ContainsKey(robotpos))
                {
                    panelsPainted++;
                }
                panels[robotpos] = (int)output.Receive();
                var turn = (int)output.Receive();
                if (turn == 1)
                {
                    robotdir = (robotdir + 1) % 4;
                }
                else if (turn == 0)
                {
                    robotdir = (robotdir - 1 + 4) % 4;
                }
                else
                {
                    throw new Exception("unexpected");
                }
                robotpos = (x: robotpos.x + dirs[robotdir].x, y: robotpos.y + dirs[robotdir].y);
                if (process.IsCompleted)
                {
                    break;
                }
                input.Post(panels.ContainsKey(robotpos) ? panels[robotpos] : 0);
            }

            var minx = panels.Min(panel => panel.Key.x);
            var miny = panels.Min(panel => panel.Key.y);
            Console.Clear();
            foreach (var panel in panels)
            {
                if (panel.Value == 1)
                {
                    // prints upside down for some reason :D
                    Console.SetCursorPosition(panel.Key.x - minx, panel.Key.y - miny);
                    Console.Write('#');
                }

            }
            return "";
        }
    }
}