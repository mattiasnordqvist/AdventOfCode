using App;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Day22Tests
    {
        [Test]
        public void Test1()
        {
            var expected = "0 1 2 3 4 5 6 7 8 9";
            var actual = Day22.SolveAll(10, 10, new string[0]);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test2()
        {
            var expected = "9 8 7 6 5 4 3 2 1 0";
            var actual = Day22.SolveAll(10, 10, new string[] { "deal into new stack" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test3()
        {
            var expected = "0 1 2 3 4 5 6 7 8 9";
            var actual = Day22.SolveAll(10, 10, new string[] { "deal into new stack", "deal into new stack" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test4()
        {
            var expected = "3 2 1 0";
            var actual = Day22.SolveAll(4, 4, new string[] { "deal into new stack" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test5()
        {
            var expected = "3 4 5 6 7 8 9 0 1 2";
            var actual = Day22.SolveAll(10, 10, new string[] { "cut 3" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test6()
        {
            var expected = "6 7 8 9 0 1 2 3 4 5";
            var actual = Day22.SolveAll(10, 10, new string[] { "cut -4" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test7()
        {
            var expected = "0 7 4 1 8 5 2 9 6 3";
            var actual = Day22.SolveAll(10, 10, new string[] { "deal with increment 3" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test8()
        {
            var expected = "0 3 6 9 2 5 8 1 4 7";
            var actual = Day22.SolveAll(10, 10, new string[] { "deal with increment 7" });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test9()
        {
            var expected = "0 1 2 3 4 5 6 7 8 9";
            var actual = Day22.SolveAll(10, 10, new string[] { "deal with increment 1" });
            Assert.AreEqual(expected, actual);
        }

    }
}
