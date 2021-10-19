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
        public void TestInsertAt()
        {
            var a1 = new char[] { 'A', 'B', 'C' };
            var l1 = a1.InsertAt('X', 2);
            Assert.AreEqual(4, l1.Count());
            Assert.AreEqual('A', l1.ElementAt(0));
            Assert.AreEqual('B', l1.ElementAt(1));
            Assert.AreEqual('X', l1.ElementAt(2));
            Assert.AreEqual('C', l1.ElementAt(3));

            // Insert into empty sequence with overflow
            var a2 = new char[] { };
            var l2 = a2.InsertAt('X', 2);
            Assert.AreEqual(1, l2.Count());
            Assert.AreEqual('X', l2.ElementAt(0));

            // Underflow
            var a3 = new char[] { 'A', 'B' };
            var l3 = a3.InsertAt('X', -1);
            Assert.AreEqual(3, l3.Count());
            Assert.AreEqual('X', l3.ElementAt(0));
            Assert.AreEqual('A', l3.ElementAt(1));
            Assert.AreEqual('B', l3.ElementAt(2));

            // Overflow
            var a4 = new char[] { 'A', 'B' };
            var l4 = a4.InsertAt('X', 12);
            Assert.AreEqual(3, l4.Count());
            Assert.AreEqual('A', l4.ElementAt(0));
            Assert.AreEqual('B', l4.ElementAt(1));
            Assert.AreEqual('X', l4.ElementAt(2));
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
