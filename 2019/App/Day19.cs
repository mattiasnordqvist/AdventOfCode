using System.Collections.Generic;

namespace App
{
    public class Day19 : Day
    {
        protected override string Part1Code(string data)
        {
            int size = 0;
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    var queue = new Queue<int>();
                    queue.Enqueue(x);
                    queue.Enqueue(y);
                    var computer = new IntCodeComputer()
                    {
                        Memory = IntCodeComputer.ParseProgram(data),
                        ReadInput = () => queue.Dequeue(),
                        WriteOutput = (o) =>
                        {
                            size += (int)o;
                        }
                    };
                    computer.Compute();
                }
            }
            return size.ToString();
        }



        protected override string Part2Code(string data)
        {
            int lastX = 0;
            int y = 10; // tractor beam looks strange in beginning. Just skip forward a bit.
            int x = lastX;
            L1: while (true)
            {
                if (!Check(data, x, y))
                {
                    x++;
                }
                else
                {
                    lastX = x;
                    L2: while (true)
                    {
                        if (Check(data, x + 99, y))
                        {
                            if (Check(data, x, y + 99))
                            {
                                return ((x * 10000) + y).ToString();
                            }
                            else
                            {
                                x++;
                                goto L2;
                            }
                        }
                        else
                        {
                            y++;
                            x = lastX;
                            goto L1;
                        }
                    }
                }
            }
        }

        private static bool Check(string data, int x, int y)
        {
            var queue = new Queue<int>();
            queue.Enqueue(x);
            queue.Enqueue(y);
            var computer = new IntCodeComputer()
            {
                Memory = IntCodeComputer.ParseProgram(data),
                ReadInput = () => queue.Dequeue(),
                WriteOutput = (o) =>
                {
                    //output = (int)o;
                }
            };

            return computer.Compute() == 1;
        }
    }
}
