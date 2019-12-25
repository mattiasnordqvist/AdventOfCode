using System;
using System.Collections.Generic;
using System.Linq;

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
                        WriteOutput = (o) => size += (int)o
                    };
                    computer.Compute();
                }
            }
            return size.ToString();
        }

        

        protected override string Part2Code(string data)
        {
            return "";
        }
    }
}
