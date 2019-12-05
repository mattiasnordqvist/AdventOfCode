using App;
using NUnit.Framework;

namespace NUnitTestProject1
{
    public class Day5Tests
    {
        [Test]
        public void TestSample1()
        {
            var memory = new int[] { 1002, 4, 3, 4, 33 };
            Day5.Compute(memory, null);
            Assert.AreEqual(99, memory[4]);
        }

        [Test]
        public void TestSample2()
        {
            var memory = new int[] { 1101, 100, -1, 4, 0 };
            Day5.Compute(memory, null);
            Assert.AreEqual(99, memory[4]);
        }
    }
}