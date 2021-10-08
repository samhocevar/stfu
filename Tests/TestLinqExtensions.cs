using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stfu.Linq;
using System.Linq;

namespace Tests2
{
    [TestClass]
    public class TestLinqExtensions
    {
        [TestMethod]
        public void TestYield()
        {
            var seq = 42.Yield();
            Assert.IsNotNull(seq);
            Assert.AreEqual(1, seq.Count());
        }

        [TestMethod]
        public void TestIntersperse()
        {
            var a1 = new int[] { 1, 2, 3 };
            var l1 = a1.Intersperse(42);
            Assert.AreEqual(5, l1.Count());
            Assert.AreEqual(1, l1.ElementAt(0));
            Assert.AreEqual(42, l1.ElementAt(1));
            Assert.AreEqual(2, l1.ElementAt(2));
            Assert.AreEqual(42, l1.ElementAt(3));
            Assert.AreEqual(3, l1.ElementAt(4));
        }
    }
}
