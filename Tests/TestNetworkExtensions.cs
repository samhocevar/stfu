using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stfu;
using System.Net;

namespace Tests2
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
