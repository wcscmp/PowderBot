using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class SubscriptionModelTest
    {
        [DataTestMethod]
        [DataRow("http://www.snow-forecast.com/resorts/Solitude", "Solitude")]
        [DataRow("http://www.snow-forecast.com/resorts/Alta/6day/mid", "Alta")]
        public void RowKeyIsCalculatedFromUserIdAndResort(string uri, string resort)
        {
            const string userId = "42";
            Assert.AreEqual(userId + resort, new SubscriptionModel(userId, uri).RowKey);
        }
    }
}
