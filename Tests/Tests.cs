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

using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        [STAThread]
        public static void Main()
        {
            TestResult();
        }

        private static void TestResult()
        {
            Stfu.Result r1 = Stfu.Result.Ok;
            bool b1 = r1.IsError; // false

            Stfu.Result r2 = Stfu.Result.Error("Oops");
            bool b2 = r2.IsError; // true

            Stfu.Result<int> r3 = (0, "OMG PROBLEM");
            bool b3 = r3.IsError; // true
            var e = r3.Message; // "OMG PROBLEM"

            Stfu.Result<int> r4 = 42;
            bool b4 = r4.IsError; // false
            int w = r4; // 42
            var v = r4.Value; // 42

            Stfu.Result<IEnumerable<int>> r5 = Stfu.Result.Ok;
            bool b5 = r5.IsError; // false
            var v5 = r5.Value; // null
        }
    }
}
