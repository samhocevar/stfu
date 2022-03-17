//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2022 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Stfu;

namespace Tests2
{
    [TestClass]
    public class TestResult
    {
        [TestMethod]
        public void TestBase()
        {
            Result r1 = Result.Ok;
            bool b1 = r1.IsError;
            Assert.IsFalse(b1);

            Result r2 = Result.Error("Oops");
            bool b2 = r2.IsError;
            Assert.IsTrue(b2);
        }

        [TestMethod]
        public void TestInt()
        {
            Result<int> r1 = (0, "OMG PROBLEM");
            bool b1 = r1.IsError;
            var e1 = r1.Message;
            int i1 = r1;
            var v1 = r1.Value;
            Assert.IsTrue(b1);
            Assert.AreEqual(e1, "OMG PROBLEM");
            Assert.AreEqual(i1, 0);
            Assert.AreEqual(v1, 0);

            Result<int> r2 = 42;
            bool b2 = r2.IsError;
            int i2 = r2;
            var v2 = r2.Value;
            Assert.IsFalse(b2);
            Assert.AreEqual(i2, 42);
            Assert.AreEqual(v2, 42);
        }

        [TestMethod]
        public void TestList()
        {
            Result<List<int>> r1 = Result.Ok;
            bool b1 = r1.IsError;
            List<int> l1 = r1;
            var v1 = r1.Value;
            Assert.IsFalse(b1);
            Assert.IsNotNull(l1); // because List<> can be constructed
            Assert.IsNotNull(v1);
            Assert.AreEqual(l1.Count, 0);
            Assert.AreEqual(v1.Count, 0);

            Result<List<int>> r2 = Result.Error("No data");
            bool b2 = r2.IsError;
            var e2 = r2.Message;
            List<int> l2 = r2;
            var v2 = r2.Value;
            Assert.IsTrue(b2);
            Assert.AreEqual(e2, "No data");
            Assert.IsNull(l2);
            Assert.IsNull(v2);
        }

        [TestMethod]
        public void TestIEnumerable()
        {
            Result<IEnumerable<int>> r1 = Result.Ok;
            bool b1 = r1.IsError;
            var v1 = r1.Value;
            Assert.IsFalse(b1);
            Assert.IsNull(v1);

            Result<IEnumerable<int>> r2 = Result.Error("Overflow");
            bool b2 = r2.IsError;
            var e2 = r2.Message;
            var v2 = r2.Value;
            Assert.IsTrue(b2);
            Assert.AreEqual(e2, "Overflow");
            Assert.IsNull(v2);
        }
    }
}
