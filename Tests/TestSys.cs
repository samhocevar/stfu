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
using Stfu;

namespace Tests2
{
    [TestClass]
    public class TestSys
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsNotNull(Sys.BuiltinGroupName);
        }
    }
}
