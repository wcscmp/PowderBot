using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebClient;

namespace UnitTests
{
    [TestClass]
    public class SnowfallCalculatorTest
    {
        [DataTestMethod]
        [DataRow("<body><div><span class=\"snowu\">cm</span><div><div><span class=\"snow\">-</span><div></body>", 0)]
        [DataRow("<body><div><span class=\"snowu\">cm</span><div><div><span class=\"snow\">-</span><div><div><span class=\"snow\">14</span><div></body>", 14)]
        [DataRow("<body><div><span class=\"snowu\">in</span><div><div><span class=\"snow\">-</span><div><div><span class=\"snow\">5.5</span><div></body>", 14)]
        public void GetGmtTest(string html, int snowfall)
        {
            Assert.AreEqual(snowfall, SnowfallCalculator.GetSnowfall(html));
        }
    }
}
