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
using System.Net;

namespace Tests
{
    [TestClass]
    public class TestNetworkExtensions
    {
        [TestMethod]
        public void Test()
        {
            var ip1 = IPAddress.Parse("192.168.1.1");
            Assert.IsTrue(ip1.IsInternal());

            var ip2 = IPAddress.Parse("123.45.67.89");
            Assert.IsFalse(ip2.IsInternal());
        }
    }
}
