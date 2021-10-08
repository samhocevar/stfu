using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Stfu;

namespace Tests2
{
    [TestClass]
    public class TestResult
    {
        [TestMethod]
        public void Test()
        {
            Result r1 = Result.Ok;
            bool b1 = r1.IsError;
            Assert.IsFalse(b1);

            Result r2 = Result.Error("Oops");
            bool b2 = r2.IsError;
            Assert.IsTrue(b2);

            Result<int> r3 = (0, "OMG PROBLEM");
            bool b3 = r3.IsError;
            var e3 = r3.Message;
            Assert.IsTrue(b3);
            Assert.AreEqual(e3, "OMG PROBLEM");

            Result<int> r4 = 42;
            bool b4 = r4.IsError;
            int i4 = r4;
            var v4 = r4.Value;
            Assert.IsFalse(b4);
            Assert.AreEqual(i4, 42);
            Assert.AreEqual(v4, 42);

            Result<List<int>> r5 = Result.Ok;
            bool b5 = r5.IsError;
            var v5 = r5.Value;
            Assert.IsFalse(b5);
            Assert.AreEqual(v5.Count, 0);

            Result<IEnumerable<int>> r6 = Result.Ok;
            bool b6 = r6.IsError;
            var v6 = r6.Value;
            Assert.IsFalse(b6);
            Assert.AreEqual(v6, null); // because IEnumerable<> cannot be constructed

            Result<IEnumerable<int>> r7 = Result.Error("Overflow");
            bool b7 = r7.IsError;
            var e7 = r7.Message;
            Assert.IsTrue(b7);
            Assert.AreEqual(e7, "Overflow");
        }
    }
}
