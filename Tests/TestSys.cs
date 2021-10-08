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
