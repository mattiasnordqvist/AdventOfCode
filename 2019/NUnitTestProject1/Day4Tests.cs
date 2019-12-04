using NUnit.Framework;

namespace NUnitTestProject1
{

    public class Day3Tests
    {
        [Test]
        public void Samples()
        {
            Assert.AreEqual("159", new App.Day3().Part1(@"R75, D30, R83, U83, L12, D49, R71, U7, L72
U62, R66, U55, R34, D71, R55, D58, R83"
));
            Assert.AreEqual("135", new App.Day3().Part1(@"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
));
            Assert.AreEqual("610", new App.Day3().Part2(@"R75,D30,R83,U83,L12,D49,R71,U7,L72
U62, R66, U55, R34, D71, R55, D58, R83"
));
            Assert.AreEqual("410", new App.Day3().Part2(@"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
));
        }
    }
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