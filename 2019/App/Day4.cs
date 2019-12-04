using System;

namespace App
{
    public class Day4 : Day
    {
        public Day4()
        {
            AddTestForPart1("111111-111111", "1");
            AddTestForPart1("223450-223450", "0");
            AddTestForPart1("123789-123789", "0");
            AddTestForPart1("200458-200458", "0");
        }



        ////Gives the next number where each digit is greater or equal to the one before it, unless n already satisfies this rule, then n is returned.
        public static int IfNotIncreasingGetNextIncreasing(int n)
        {
            var newNr = 0;
            var nrOfDigits = (int)Math.Log10(n);
            var prevDigit = -1;
            var fillWithThisDigit = -1;
            for (int i = nrOfDigits; i >= 0; i--)
            {
                if (i == nrOfDigits)
                {
                    prevDigit = Digit(i, n);
                    newNr += prevDigit * (int)Math.Pow(10, i);
                }
                else
                {
                    if (fillWithThisDigit != -1)
                    {
                        newNr += fillWithThisDigit * (int)Math.Pow(10, i);
                    }
                    else
                    {
                        if (prevDigit > Digit(i, n))
                        {
                            fillWithThisDigit = prevDigit;
                        }
                        else
                        {
                            prevDigit = Digit(i, n);
                        }
                        newNr += prevDigit * (int)Math.Pow(10, i);
                    }

                }

            }
            return newNr;
        }

        public static int Digit(int nr, int from)
        {
            return from / (int)Math.Pow(10, nr) % 10;
        }

        protected override string Part1Code(string data)
        {
            var from = int.Parse(data.Split('-')[0]);
            var to = int.Parse(data.Split('-')[1]);
            var count = 0;
            for (int candidate = IfNotIncreasingGetNextIncreasing(from); candidate <= to; candidate = IfNotIncreasingGetNextIncreasing(candidate+1))
            {
                if (AnyTwoAdjecent(candidate.ToString()))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        private bool AnyTwoAdjecent(string v)
        {
            for (int i = 0; i < v.Length - 1; i++)
            {
                if (v[i] == v[i + 1])
                {
                    return true;
                }
            }
            return false;
        }

        private bool AnyTwoAdjecentButNotThree(string v)
        {
            for (int i = 0; i < v.Length - 1; i++)
            {
                if (v[i] == v[i + 1])
                {
                    if ((i > 0 && v[i] == v[i - 1]) || (i < v.Length - 2 && v[i] == v[i + 2]))
                    {
                        continue;
                    }
                    return true;
                }
            }
            return false;
        }

        protected override string Part2Code(string data)
        {
            var from = int.Parse(data.Split('-')[0]);
            var to = int.Parse(data.Split('-')[1]);
            var count = 0;
            for (int candidate = IfNotIncreasingGetNextIncreasing(from); candidate <= to; candidate = IfNotIncreasingGetNextIncreasing(candidate+1))
            {
                if (AnyTwoAdjecentButNotThree(candidate.ToString()))
                {
                    count++;
                }
            }
            return count.ToString();
        }
    }
}
