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
        /// <summary>
        /// Test creating and commiting an empty file.
        /// </summary>
        [TestMethod]
        public void TestCreateEmpty()
        {
            string temp_path = null;

            using (var writer = new AtomicFileWriter(TestFileName))
            {
                Assert.IsNotNull(writer);
                Assert.IsNotNull(writer.Path);
                Assert.IsNotNull(writer.TemporaryPath);

                Assert.AreEqual(writer.Path, TestFileName);
                Assert.AreNotEqual(writer.TemporaryPath, TestFileName);

                // While an AtomicFileWriter exists, the temporary path is present
                temp_path = writer.TemporaryPath;
                Assert.IsTrue(File.Exists(temp_path));

                writer.Commit();
            }

            // When the AtomicFileWriter no longer exists, the temporary path is no
            // longer present, and the final path is present
            Assert.IsFalse(File.Exists(temp_path));
            Assert.IsTrue(File.Exists(TestFileName));
        }

        /// <summary>
        /// Test aborting a file creation attempt. The new file creating and commiting an empty file.
        /// </summary>
        [TestMethod]
        public void TestCreateNoCommit()
        {
            string temp_path = null;

            using (var writer = new AtomicFileWriter(TestFileName))
            {
                // Intentionally no Commit() call
            }

            // When the AtomicFileWriter no longer exists, the temporary path is no longer present
            Assert.IsFalse(File.Exists(temp_path));
        }

        [TestMethod]
        public void TestWriteChar()
        {
            using (var writer = new AtomicFileWriter(TestFileName))
            {
                foreach (char c in Data1)
                    writer.Write(c);
                writer.Commit();
            }

            // Verify that the file exists and contains what we expected
            Assert.IsTrue(File.Exists(TestFileName));
            string contents = File.ReadAllText(TestFileName);
            Assert.AreEqual(Data1, contents);
        }

        [TestMethod]
        public void TestWriteString()
        {
            using (var writer = new AtomicFileWriter(TestFileName))
            {
                writer.Write(Data1);
                writer.Write(Data2);
                writer.Commit();
            }

            // Verify that the file exists and contains what we expected
            Assert.IsTrue(File.Exists(TestFileName));
            string contents = File.ReadAllText(TestFileName);
            Assert.AreEqual(Data1 + Data2, contents);
        }

        [TestMethod]
        public void TestAppend()
        {
            using (var writer = new AtomicFileWriter(TestFileName))
            {
                writer.Write(Data1);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(TestFileName));

            using (var writer = new AtomicFileWriter(TestFileName, append: true))
            {
                writer.Write(Data2);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(TestFileName));

            string contents = File.ReadAllText(TestFileName);
            Assert.AreEqual(Data1 + Data2, contents);
        }

        [TestMethod]
        public void TestFail()
        {
            using (var writer = new AtomicFileWriter(TestFileName))
            {
                writer.Write(Data1);
                writer.Commit();
            }
            Assert.IsTrue(File.Exists(TestFileName));

            using (var writer = new AtomicFileWriter(TestFileName))
            {
                writer.Write(Data2);
            }
            Assert.IsTrue(File.Exists(TestFileName));

            string contents = File.ReadAllText(TestFileName);
            Assert.AreEqual(Data1, contents);
        }

        private readonly string TestFileName = "test.txt";
        private readonly string Data1 = "Hello";
        private readonly string Data2 = "World";
    }
}
