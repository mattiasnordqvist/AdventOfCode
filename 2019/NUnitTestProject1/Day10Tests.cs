using System;
using App;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Day10Tests
    {
        [Test]
        public void Angles1()
        {
            Assert.AreEqual(0, Day10.Angle((1, 1), (2, 1)));
        }
        [Test]
        public void Angles2()
        {
            Assert.AreEqual(45, Day10.Angle((1, 1), (2, 0)));
        }
        [Test]
        public void Angles3()
        {
            Assert.AreEqual(90, Day10.Angle((1, 1), (1, 0)));
        }
        [Test]
        public void Angles4()
        {
            Assert.AreEqual(135, Day10.Angle((1, 1), (0, 0)));
        }
        [Test]
        public void Angles5()
        {
            Assert.AreEqual(180, Day10.Angle((1, 1), (0, 1)));
        }
        [Test]
        public void Angles6()
        {
            Assert.AreEqual(225, Day10.Angle((1, 1), (0, 2)));
        }
        [Test]
        public void Angles7()
        {
            Assert.AreEqual(270, Day10.Angle((1, 1), (1, 2)));
        }
        [Test]
        public void Angles8()
        {
            Assert.AreEqual(315, Day10.Angle((1, 1), (2, 2)));
        }
        [Test]
        public void Angles10()
        {

            Assert.AreEqual(135, Day10.Angle((2, 2), (0, 0)));
        }
        [Test]
        public void Angles11()
        {
            Assert.AreEqual(135, Day10.Angle((2, 2), (1, 1)));
        }
        [Test]
        public void Angles12()
        {
            Assert.AreEqual(90, Day10.Angle((2, 2), (2, 0)));
        }
        [Test]
        public void Angles13()
        {
            Assert.AreEqual(90, Day10.Angle((2, 2), (2, 1)));
        }
        [Test]
        public void Angles14()
        {

            Assert.AreEqual(45, Day10.Angle((2, 2), (3, 1)));
        }
        [Test]
        public void Angles15()
        {
            Assert.AreEqual(45, Day10.Angle((2, 2), (4, 0)));
        }
        [Test]
        public void Angles16()
        {
            Assert.AreEqual(0, Day10.Angle((2, 2), (4, 2)));
        }
        [Test]
        public void Angles17()
        {
            Assert.AreEqual(26.565, Day10.Angle((2, 2), (4, 1)));
        }
        [Test]
        public void Angles18()
        {
            Assert.AreEqual(180 - 26.565, Day10.Angle((2, 2), (0, 1)));
        }
        [Test]
        public void Angles19()
        {
            Assert.AreEqual(180 + 26.565, Day10.Angle((2, 2), (0, 3)));
        }
    }
}
