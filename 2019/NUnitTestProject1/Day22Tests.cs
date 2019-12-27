using App;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Day22Tests
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(0, Day22.SlowInc(0, 1, 10));
            Assert.AreEqual(1, Day22.SlowInc(1, 1, 10));
            Assert.AreEqual(2, Day22.SlowInc(2, 1, 10));
            Assert.AreEqual(3, Day22.SlowInc(3, 1, 10));
            Assert.AreEqual(4, Day22.SlowInc(4, 1, 10));
            Assert.AreEqual(5, Day22.SlowInc(5, 1, 10));
            Assert.AreEqual(6, Day22.SlowInc(6, 1, 10));
            Assert.AreEqual(7, Day22.SlowInc(7, 1, 10));
            Assert.AreEqual(8, Day22.SlowInc(8, 1, 10));
            Assert.AreEqual(9, Day22.SlowInc(9, 1, 10));
        }
        [Test]
        public void Test1F()
        {
            Assert.AreEqual(0, Day22.FastInc(0, 1, 10));
            Assert.AreEqual(1, Day22.FastInc(1, 1, 10));
            Assert.AreEqual(2, Day22.FastInc(2, 1, 10));
            Assert.AreEqual(3, Day22.FastInc(3, 1, 10));
            Assert.AreEqual(4, Day22.FastInc(4, 1, 10));
            Assert.AreEqual(5, Day22.FastInc(5, 1, 10));
            Assert.AreEqual(6, Day22.FastInc(6, 1, 10));
            Assert.AreEqual(7, Day22.FastInc(7, 1, 10));
            Assert.AreEqual(8, Day22.FastInc(8, 1, 10));
            Assert.AreEqual(9, Day22.FastInc(9, 1, 10));
        }
        [Test]
        public void Test3F()
        {
            Assert.AreEqual(0, Day22.FastInc(0, 3, 10));
            Assert.AreEqual(7, Day22.FastInc(1, 3, 10));
            Assert.AreEqual(4, Day22.FastInc(2, 3, 10));
            Assert.AreEqual(1, Day22.FastInc(3, 3, 10));
            Assert.AreEqual(8, Day22.FastInc(4, 3, 10));
            Assert.AreEqual(5, Day22.FastInc(5, 3, 10));
            Assert.AreEqual(2, Day22.FastInc(6, 3, 10));
            Assert.AreEqual(9, Day22.FastInc(7, 3, 10));
            Assert.AreEqual(6, Day22.FastInc(8, 3, 10));
            Assert.AreEqual(3, Day22.FastInc(9, 3, 10));
        }
        [Test]
        public void Test3()
        {
            Assert.AreEqual(0, Day22.SlowInc(0, 3, 10));
            Assert.AreEqual(7, Day22.SlowInc(1, 3, 10));
            Assert.AreEqual(4, Day22.SlowInc(2, 3, 10));
            Assert.AreEqual(1, Day22.SlowInc(3, 3, 10));
            Assert.AreEqual(8, Day22.SlowInc(4, 3, 10));
            Assert.AreEqual(5, Day22.SlowInc(5, 3, 10));
            Assert.AreEqual(2, Day22.SlowInc(6, 3, 10));
            Assert.AreEqual(9, Day22.SlowInc(7, 3, 10));
            Assert.AreEqual(6, Day22.SlowInc(8, 3, 10));
            Assert.AreEqual(3, Day22.SlowInc(9, 3, 10));
        }

        [Test]
        public void Test7()
        {
            Assert.AreEqual(0, Day22.SlowInc(0, 7, 10));
            Assert.AreEqual(3, Day22.SlowInc(1, 7, 10));
            Assert.AreEqual(6, Day22.SlowInc(2, 7, 10));
            Assert.AreEqual(9, Day22.SlowInc(3, 7, 10));
            Assert.AreEqual(2, Day22.SlowInc(4, 7, 10));
            Assert.AreEqual(5, Day22.SlowInc(5, 7, 10));
            Assert.AreEqual(8, Day22.SlowInc(6, 7, 10));
            Assert.AreEqual(1, Day22.SlowInc(7, 7, 10));
            Assert.AreEqual(4, Day22.SlowInc(8, 7, 10));
            Assert.AreEqual(7, Day22.SlowInc(9, 7, 10));
        }


        [Test]
        public void Test7F()
        {
            Assert.AreEqual(0, Day22.FastInc(0, 7, 10));
            Assert.AreEqual(3, Day22.FastInc(1, 7, 10));
            Assert.AreEqual(6, Day22.FastInc(2, 7, 10));
            Assert.AreEqual(9, Day22.FastInc(3, 7, 10));
            Assert.AreEqual(2, Day22.FastInc(4, 7, 10));
            Assert.AreEqual(5, Day22.FastInc(5, 7, 10));
            Assert.AreEqual(8, Day22.FastInc(6, 7, 10));
            Assert.AreEqual(1, Day22.FastInc(7, 7, 10));
            Assert.AreEqual(4, Day22.FastInc(8, 7, 10));
            Assert.AreEqual(7, Day22.FastInc(9, 7, 10));
        }

        [Test]
        public void Test9()
        {
            Assert.AreEqual(0, Day22.SlowInc(0, 9, 10));
            Assert.AreEqual(9, Day22.SlowInc(1, 9, 10));
            Assert.AreEqual(8, Day22.SlowInc(2, 9, 10));
            Assert.AreEqual(7, Day22.SlowInc(3, 9, 10));
            Assert.AreEqual(6, Day22.SlowInc(4, 9, 10));
            Assert.AreEqual(5, Day22.SlowInc(5, 9, 10));
            Assert.AreEqual(4, Day22.SlowInc(6, 9, 10));
            Assert.AreEqual(3, Day22.SlowInc(7, 9, 10));
            Assert.AreEqual(2, Day22.SlowInc(8, 9, 10));
            Assert.AreEqual(1, Day22.SlowInc(9, 9, 10));
        }

        [Test]
        public void Test9F()
        {
            Assert.AreEqual(0, Day22.FastInc(0, 9, 10));
            Assert.AreEqual(9, Day22.FastInc(1, 9, 10));
            Assert.AreEqual(8, Day22.FastInc(2, 9, 10));
            Assert.AreEqual(7, Day22.FastInc(3, 9, 10));
            Assert.AreEqual(6, Day22.FastInc(4, 9, 10));
            Assert.AreEqual(5, Day22.FastInc(5, 9, 10));
            Assert.AreEqual(4, Day22.FastInc(6, 9, 10));
            Assert.AreEqual(3, Day22.FastInc(7, 9, 10));
            Assert.AreEqual(2, Day22.FastInc(8, 9, 10));
            Assert.AreEqual(1, Day22.FastInc(9, 9, 10));
        }
    }
}
