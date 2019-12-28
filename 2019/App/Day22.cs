using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{

    public class Day22 : Day
    {
        /*for part 1 and testing*/
        public static string SolveAll(int i, long s, string[] operations)
        {
            return Enumerable.Range(0, i).Select(x => Solve(x, s, operations)).Aggregate("", (a, b) => a + " " + b).Trim();
        }
        public static long Solve(long i, long s, string[] operations)
        {
            (long a, long b) l = (1, 0);
            foreach (var operation in operations)
            {
                if (operation == "deal into new stack")
                {
                    l = Reverse(l, s);
                }
                else if (operation.StartsWith("cut"))
                {
                    var c = long.Parse(operation.Substring(4));
                    l = Cut(l, c);
                }
                else if (operation.StartsWith("deal with increment"))
                {
                    var inc = long.Parse(operation.Substring(19));
                    l = Deal(l, inc);
                }
            }
            return mod(l.a * i + l.b, s);

        }

        public static (long a, long b) Reverse((long a, long b) l, long s) => (l.a * -1, -l.b + s - 1);
        public static (long a, long b) Cut((long a, long b) l, long c) => (l.a, l.b + c);
        public static (long a, long b) Deal((long a, long b) l, long inc) => (l.a * inc, l.b); /*not sure what im doing wrong here but I think Im applying it backwards. which should be fine since im using it backwards, bit still doesnt work.*/

        public static long mod(long x, long m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }


        protected override string Part1Code(string data)
        {
            return "";
            //var deck = new Deck(10007);

            //foreach (var line in data.Lines())
            //{
            //    if (line.StartsWith("cut"))
            //    {
            //        deck.cut(int.Parse(line.Substring(4)));
            //    }
            //    else if (line.StartsWith("deal with increment"))
            //    {
            //        deck.dealWithIncrement(int.Parse(line.Substring(19)));
            //    }
            //    else if (line == "deal into new stack")
            //    {
            //        deck.dealIntoNewStack();
            //    }
            //}
            ////deck.print();
            //int i = 0;
            //foreach (var c in deck.cards)
            //{
            //    if (c == 2019)
            //    {
            //        return i.ToString();
            //    }
            //    else
            //    {
            //        i++;
            //    }
            //}
            //throw new Exception("asdf");

        }

        protected override string Part2Code(string data)
        {
            return "";
            var size = 119315717514047L;
            var iterations = 101741582076661L;
            long i = 2020;


            //            data = @"deal into new stack
            //cut -2
            //deal with increment 7
            //cut 8
            //cut -4
            //deal with increment 7
            //cut 3
            //deal with increment 9
            //deal with increment 3
            //cut -1";

            //            size = 10;
            //            i = 0;
            //            iterations = 1;

            //for (long iteration = 0; iteration < iterations; iteration++)
            //{
            //    foreach (var line in data.Lines().Reverse())
            //    {
            //        if (line.StartsWith("cut"))
            //        {
            //            i = Cut(i, int.Parse(line.Substring(4)), size);
            //        }
            //        else if (line.StartsWith("deal with increment"))
            //        {
            //            i = FastInc(i, int.Parse(line.Substring(19)), size);
            //        }
            //        else if (line == "deal into new stack")
            //        {
            //            i = Rev(i, size);
            //        }
            //    }
            //}
            //Console.WriteLine(i);
            //return "";


        }

        //public static (long, long) Rev(long i, long size) => size - 1 - i;

        //public static long Cut(long i, long n, long size) => n >= 0 ? ((i + n) % size) : Rev(Cut(Rev(i, size), -n, size), size);
    }

}
