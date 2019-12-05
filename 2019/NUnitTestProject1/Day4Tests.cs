using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Day4Tests
    {
        [Test]
        public void Samples()
        {
            Assert.AreEqual("1",new App.Day4().Part1("111111-111111"));
            Assert.AreEqual("0",new App.Day4().Part1("223450-223450"));
            Assert.AreEqual("0",new App.Day4().Part1("123789-123789"));
            Assert.AreEqual("0",new App.Day4().Part1("200458-200458"));
        }
        [Test]
        public void Tests()
        {
            Assert.AreEqual(4, App.Day4.Digit(0, 1234));
            Assert.AreEqual(3, App.Day4.Digit(1, 1234));
            Assert.AreEqual(1, App.Day4.Digit(3, 1234));

            Assert.AreEqual(8, App.Day4.IfNotIncreasingGetNextIncreasing(8));
            Assert.AreEqual(9, App.Day4.IfNotIncreasingGetNextIncreasing(9));
            Assert.AreEqual(11, App.Day4.IfNotIncreasingGetNextIncreasing(10));
            Assert.AreEqual(599, App.Day4.IfNotIncreasingGetNextIncreasing(590));
            Assert.AreEqual(599, App.Day4.IfNotIncreasingGetNextIncreasing(599));
            Assert.AreEqual(137777, App.Day4.IfNotIncreasingGetNextIncreasing(137683));

            Assert.AreEqual(9, App.Day4.IfNotIncreasingGetNextIncreasing(8+1));
            Assert.AreEqual(11, App.Day4.IfNotIncreasingGetNextIncreasing(9+1));
            Assert.AreEqual(22, App.Day4.IfNotIncreasingGetNextIncreasing(19+1));
            Assert.AreEqual(666, App.Day4.IfNotIncreasingGetNextIncreasing(599+1));
            Assert.AreEqual(599, App.Day4.IfNotIncreasingGetNextIncreasing(589+1));
            Assert.AreEqual(599, App.Day4.IfNotIncreasingGetNextIncreasing(590+1));
        }
    }
}