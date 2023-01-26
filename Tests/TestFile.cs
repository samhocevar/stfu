//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2023 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stfu;

namespace Tests
{
    [TestClass]
    public class TestAtomicFileWriter
    {
        [TestMethod]
        public void TestCreate()
        {
            string final_path = "test.tmp";
            string temp_path = null;

            using (var writer = new AtomicFileWriter(final_path))
            {
                Assert.IsNotNull(writer);
                Assert.IsNotNull(writer.Path);
                Assert.IsNotNull(writer.TemporaryPath);

                Assert.AreEqual(writer.Path, final_path);
                Assert.AreNotEqual(writer.TemporaryPath, final_path);

                temp_path = writer.TemporaryPath;
                Assert.IsTrue(System.IO.File.Exists(temp_path));
            }

            Assert.IsFalse(System.IO.File.Exists(temp_path));
            Assert.IsTrue(System.IO.File.Exists(final_path));
        }
    }
}
