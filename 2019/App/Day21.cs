using System;

namespace App
{
    public class Day21 : Day
    {
        protected override string Part1Code(string data)
        {
            var springScript = ASCII("NOT A J;NOT B T;OR T J;NOT C T;OR T J;AND D J;WALK;");
            int i = 0;
            var comp = new IntCodeComputer();
            comp.Memory = IntCodeComputer.ParseProgram(data);
            comp.ReadInput = () => springScript[i++];
            comp.WriteOutput = o => Console.Write((char)o);
            return comp.Compute().ToString();
        }

        private string ASCII(string v)
        {
            return v.Replace(';', (char)10);
        }

        protected override string Part2Code(string data)
        {
            var springScript = ASCII("NOT A J;NOT B T;OR T J;NOT C T;OR T J;AND D J;NOT E T;NOT T T;OR H T;AND T J;RUN; ");
            int i = 0;
            var comp = new IntCodeComputer();
            comp.Memory = IntCodeComputer.ParseProgram(data);
            comp.ReadInput = () => springScript[i++];
            comp.WriteOutput = o => Console.Write((char)o);
            return comp.Compute().ToString();
        }
    }
}
