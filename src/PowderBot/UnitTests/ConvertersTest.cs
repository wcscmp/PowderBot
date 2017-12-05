using Common.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ConvertersTest
    {
        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 3)]
        [DataRow(2, 5)]
        public void IntInchToCmTest(int inch, int cm)
        {
            Assert.AreEqual(cm, inch.InchToCm());
        }

        [DataTestMethod]
        [DataRow(0.0f, 0)]
        [DataRow(5.5f, 14)]
        [DataRow(1.2f, 3)]
        public void FloatInchToCmTest(float inch, int cm)
        {
            Assert.AreEqual(cm, inch.InchToCm());
        }
    }
}
