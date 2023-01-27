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
            const string chatId = "13";
            const string userId = "42";

            Assert.AreEqual(chatId + resort, new SubscriptionModel(chatId, uri, userId).RowKey);
        }

        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(7, true)]
        [DataRow(-7, true)]
        [DataRow(10, false)]
        [DataRow(-10, true)]
        [DataRow(20, false)]
        [DataRow(-20, false)]
        public void UpdatedTodayTest(int gmt, bool expected)
        {
            var user = new UserModel("42")
            {
                Gmt = gmt
            };
            var now = new DateTimeOffset(2017, 10, 10, 16, 0, 0, new TimeSpan());
            var sub = new SubscriptionModel
            {
                Timestamp = now
            };
            Assert.AreEqual(expected, sub.UpdatedToday(user, now));
        }
    }
}
