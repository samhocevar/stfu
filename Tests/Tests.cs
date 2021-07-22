//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2017—2021 Sam Hocevar <sam@hocevar.net>
//
//  This program is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using Stfu;
using System;
using System.Collections.Generic;
using System.Net;

namespace Tests
{
    public class Tests
    {
        [STAThread]
        public static void Main()
        {
            TestResult();
            TestNetwork();
        }

        private static void TestResult()
        {
            Result r1 = Result.Ok;
            bool b1 = r1.IsError; // false

            Result r2 = Result.Error("Oops");
            bool b2 = r2.IsError; // true

            Result<int> r3 = (0, "OMG PROBLEM");
            bool b3 = r3.IsError; // true
            var e3 = r3.Message; // "OMG PROBLEM"

            Result<int> r4 = 42;
            bool b4 = r4.IsError; // false
            int w = r4; // 42
            var v = r4.Value; // 42

            Result<List<int>> r5 = Result.Ok;
            bool b5 = r5.IsError; // false
            var v5 = r5.Value; // List<int>{}

            Result<IEnumerable<int>> r6 = Result.Ok;
            bool b6 = r6.IsError; // false
            var v6 = r6.Value; // null, because IEnumerable<> cannot be constructed

            Result<IEnumerable<int>> r7 = Result.Error("Overflow");
            bool b7 = r7.IsError; // true
            var e7 = r7.Message; // "Overflow"
        }

        private static void TestNetwork()
        {
            var ip1 = IPAddress.Parse("192.168.1.1");
            bool b1 = ip1.IsInternal(); // true

            var ip2 = IPAddress.Parse("123.45.67.89");
            bool b2 = ip2.IsInternal(); // false
        }
    }
}
