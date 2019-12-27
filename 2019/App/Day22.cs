using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{

    public class DDList : IEnumerable<long>
    {
        Node First = null;
        Node Last = null;
        bool Reversed = false;

        public void AddLast(long value)
        {
            if (!Reversed)
            {
                if (First == null)
                {
                    First = new Node { Value = value };
                    Last = First;
                }
                else
                {
                    Last.Next = new Node { Value = value };
                    Last.Next.Prev = Last;
                    Last = Last.Next;
                }
            }
            else
            {
                if (Last == null)
                {
                    Last = new Node { Value = value };
                    First = Last;
                }
                else
                {
                    First.Prev = new Node { Value = value };
                    First.Prev.Next = First;
                    First = First.Prev;
                }
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            if (!Reversed)
            {
                var c = First;
                while (c != null)
                {
                    yield return c.Value;
                    c = c.Next;
                }
            }
            else
            {
                var c = Last;
                while (c != null)
                {
                    yield return c.Value;
                    c = c.Prev;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Reverse()
        {
            Reversed = !Reversed;
        }

        internal void Cut(long n)
        {
            // this gets complicated, let's try the other way around. given an index, apply changes to index
            //if (n >= 0)
            //{
            //    if (!Reversed)
            //    {
            //        var l = Last;
            //        var f = First;

            //        l.Next = f;
            //        f.Prev = l;

            //        First = 
            //    }
            //}
            //if (n < 0)
            //{
            //    Reverse();
            //    Cut(-n);
            //    Reverse();
            //}
        }
    }

    public class Node
    {
        public long Value;
        public Node Next;
        public Node Prev;
    }

    public class Deck
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in cards)
            {
                sb.Append(c + " ");
            }
            return sb.ToString().Trim();
        }
        public DDList cards;
        public Deck(long size)
        {
            cards = new DDList();
            for (long i = 0; i < size; i++)
            {
                cards.AddLast(i);
            }
        }

        public void dealIntoNewStack()
        {
            cards.Reverse();
        }

        public void cut(long n)
        {
            cards.Cut(n);

        }
        public void dealWithIncrement(long n)
        {
            //long[] newDeck = new long[cards.Length];
            //long newPos = 0;
            //for (long i = 0; i < cards.Length; i++)
            //{
            //    newDeck[newPos] = cards[i];
            //    newPos += n;
            //    newPos = newPos % cards.Length;
            //}
            //cards = newDeck;
        }

        internal void print()
        {
            foreach (var c in cards)
            {
                Console.Write(c + " ");
            }
        }
    }
    public class Day22 : Day
    {
        protected override string Part1Code(string data)
        {
            /* destroyed by changes for part 2 */
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

            for (long iteration = 0; iteration < iterations; iteration++)
            {
                foreach (var line in data.Lines().Reverse())
                {
                    if (line.StartsWith("cut"))
                    {
                        i = Cut(i, int.Parse(line.Substring(4)), size);
                    }
                    else if (line.StartsWith("deal with increment"))
                    {
                        i = FastInc(i, int.Parse(line.Substring(19)), size);
                    }
                    else if (line == "deal into new stack")
                    {
                        i = Rev(i, size);
                    }
                }

                //if(iteration % 10000 == 0)
                //{
                //    Console.WriteLine((iteration/(double)iterations)*100);
                //}
            }
            Console.WriteLine(i);
            return "";


        }
        public static long Rev(long i, long size) => size - 1 - i;

        public static long Cut(long i, long n, long size) => n >= 0 ? ((i + n) % size) : Rev(Cut(Rev(i, size), -n, size), size);


        public static long SlowInc(long i, long n, long s)
        {
            long sum = 0;
            long l = 0;
            while (true)
            {
                if (sum == i)
                {
                    return l;
                }
                l++;
                sum += n;
                if (sum > s)
                {
                    sum -= s;
                }
            }
        }

        public static long FastInc(long i, long n, long s)
        {
        }
    }

}
