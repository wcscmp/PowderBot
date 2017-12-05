using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GmtHelperTest
    {
        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(1511791122069L, 1511791115536L, 2)]
        [DataRow(1512365999L + 10, 1512383999L, -5)]
        [DataRow(1512365999L - 10, 1512383999L, -5)]
        public void GetGmtTest(long userLocalTime, long timestamp, int gmt)
        {
            Assert.AreEqual(gmt, GmtHelper.GetGmt(userLocalTime, timestamp));
        }
    }
}
