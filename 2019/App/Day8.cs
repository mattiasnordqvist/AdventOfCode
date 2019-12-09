using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day8 : Day
    {
        protected override string Part1Code(string data)
        {
            int w = 25;
            int h = 6;
            List<string> layers = new List<string>();
            for(int i = 0; w * h * i < data.Trim().Length; i++)
            {
                layers.Add(new string(data.Skip(i * w * h).Take(w * h).ToArray()));
            }
            var layer = layers.OrderBy(x => Count(x, '0')).First();

            return (Count(layer, '1') * Count(layer, '2')).ToString();
        }

        private int Count(string layer, char what)
        {
            return layer.Where(x => x == what).Count();
        }

        protected override string Part2Code(string data)
        {
            int w = 25;
            int h = 6;
            List<string> layers = new List<string>();
            for (int i = 0; w * h * i < data.Trim().Length; i++)
            {
                layers.Add(new string(data.Skip(i * w * h).Take(w * h).ToArray()));
            }

            string image = "";
            for(int i = 0; i < w * h; i++)
            {
                image+=GetPixel(layers, i);
            }

            Print(image, w, h);

            return "";
        }

        private void Print(string image, int w, int h)
        {
            for(int y = 0; y < h; y++)
            {
                Console.WriteLine(new string(image.Skip(y * w).Take(w).ToArray()).Replace('0', ' ').Replace('1', '#'));
            }
        }

        private char GetPixel(IEnumerable<string> layers, int i)
        {
            foreach(var layer in layers)
            {
                if(layer[i] != '2')
                {
                    return layer[i];
                }
            }
            throw new Exception("unexpected");
        }
    }
}
