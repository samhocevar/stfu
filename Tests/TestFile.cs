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
using System.IO;

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

            // While an AtomicFileWriter exists, the temporary path is present
            using (var writer = new AtomicFileWriter(final_path))
            {
                Assert.IsNotNull(writer);
                Assert.IsNotNull(writer.Path);
                Assert.IsNotNull(writer.TemporaryPath);

                Assert.AreEqual(writer.Path, final_path);
                Assert.AreNotEqual(writer.TemporaryPath, final_path);

                temp_path = writer.TemporaryPath;
                Assert.IsTrue(File.Exists(temp_path));

                writer.Commit();
            }

            // When the AtomicFileWriter no longer exists, the temporary path is no
            // longer present, and the final path exists
            Assert.IsFalse(File.Exists(temp_path));
            Assert.IsTrue(File.Exists(final_path));
        }

        [TestMethod]
        public void TestCreateNoCommit()
        {
            string final_path = "test.tmp";
            string temp_path = null;

            // While an AtomicFileWriter exists, the temporary path is present
            using (var writer = new AtomicFileWriter(final_path))
            {
                Assert.IsNotNull(writer);
                Assert.IsNotNull(writer.Path);
                Assert.IsNotNull(writer.TemporaryPath);

                Assert.AreEqual(writer.Path, final_path);
                Assert.AreNotEqual(writer.TemporaryPath, final_path);

                temp_path = writer.TemporaryPath;
                Assert.IsTrue(File.Exists(temp_path));
            }

            // When the AtomicFileWriter no longer exists, the temporary path is no
            // longer present, and the final path exists
            Assert.IsFalse(File.Exists(temp_path));
            Assert.IsTrue(File.Exists(final_path));
        }

        [TestMethod]
        public void TestWrite()
        {
            string path = "test.tmp";
            string data1 = "Hello";
            string data2 = "World";

            using (var writer = new AtomicFileWriter(path))
            {
                writer.Write(data1);
                writer.Write(data2);
                writer.Commit();
            }

            // Verify that the file exists and contains what we expected
            Assert.IsTrue(File.Exists(path));
            string contents = File.ReadAllText(path);
            Assert.AreEqual(data1 + data2, contents);
        }

        [TestMethod]
        public void TestAppend()
        {
            string path = "test.tmp";
            string data1 = "Hello";
            string data2 = "World";

            using (var writer = new AtomicFileWriter(path))
            {
                writer.Write(data1);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(path));

            using (var writer = new AtomicFileWriter(path, append: true))
            {
                writer.Write(data2);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(path));

            string contents = File.ReadAllText(path);
            Assert.AreEqual(data1 + data2, contents);
        }

        [TestMethod]
        public void TestFail()
        {
            string path = "test.tmp";
            string data1 = "Hello";
            string data2 = "World";

            using (var writer = new AtomicFileWriter(path))
            {
                writer.Write(data1);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(path));

            using (var writer = new AtomicFileWriter(path))
            {
                writer.Write(data2);
            }
            Assert.IsTrue(File.Exists(path));

            string contents = File.ReadAllText(path);
            Assert.AreEqual(data1, contents);
        }
    }
}
