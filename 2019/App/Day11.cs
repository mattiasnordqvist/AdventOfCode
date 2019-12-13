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

            var comp = new AsyncIntCodeComputer { Memory = memory };
            comp.Input.Post(0);
            var process = Task.Run(() => comp.Compute());
            var panelsPainted = 0;
            while (true)
            {
                if (!panels.ContainsKey(robotpos))
                {
                    panelsPainted++;
                }
                panels[robotpos] = (int)comp.Output.Receive();
                var turn = (int)comp.Output.Receive();
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
                comp.Input.Post(panels.ContainsKey(robotpos) ? panels[robotpos] : 0);
            }
            return panelsPainted.ToString();
        }

        protected override string Part2Code(string data)
        {
            Dictionary<(int x, int y), int> panels = new Dictionary<(int x, int y), int>();
            var robotpos = (x: 0, y: 0);
            var dirs = new (int x, int y)[] { (x: 0, y: 1), (x: 1, y: 0), (x: 0, y: -1), (x: -1, y: 0) };
            var robotdir = 0;
            var memory = data.FromCommaSep().Select(long.Parse).ToArray();

            var comp = new AsyncIntCodeComputer { Memory = memory };
            comp.Input.Post(0);
            var process = Task.Run(() => comp.Compute());
            var panelsPainted = 0;
            while (true)
            {
                if (!panels.ContainsKey(robotpos))
                {
                    panelsPainted++;
                }
                panels[robotpos] = (int)comp.Output.Receive();
                var turn = (int)comp.Output.Receive();
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
                comp.Input.Post(panels.ContainsKey(robotpos) ? panels[robotpos] : 0);
            }

            var minx = panels.Min(panel => panel.Key.x);
            var miny = panels.Min(panel => panel.Key.y);
            foreach (var panel in panels)
            {
                if (panel.Value == 1)
                {
                    // prints upside down for some reason :D
                    Console.SetCursorPosition(panel.Key.x - minx, panel.Key.y - miny+5);
                    Console.Write('#');
                }

            }
            return "";
        }
    }
}