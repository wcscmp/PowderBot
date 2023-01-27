using BusinessLogic;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebClient;

namespace UnitTests
{
    [TestClass]
    public class SnowfallCheckerTest
    {
        private readonly UserModel _user = new UserModel("42");
        private readonly SubscriptionModel _subscription
            = new SubscriptionModel("13", "http://www.snow-forecast.com/resorts/Alta/6day/mid", "42")
            {
                Snowfall = 5
            };

        [TestMethod]
        public async Task EmptySubscriptionListIsAcceptasded()
        {
            var mock = new Mock<ISnowForecastClient>();
            mock
                .Setup(m => m.GetSnowfall(It.IsAny<IEnumerable<string>>()))
                .Callback<IEnumerable<string>>(uris => Assert.IsFalse(uris.Any()))
                .ReturnsAsync(new (string Uri, int Snowfall)[] { });
            var checker = new SnowfallChecker(mock.Object);
            var snowfall = await checker.Check(new SubscriptionModel[] { });
            Assert.IsFalse(snowfall.Any());
            mock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(10, 1)]
        [DataRow(1, 0)]
        public async Task SnowfallForecastIsChecked(int snowfallForecast, int uriCount)
        {
            var mock = new Mock<ISnowForecastClient>();
            mock
                .Setup(m => m.GetSnowfall(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(
                    new (string Uri, int Snowfall)[] { (_subscription.Uri, snowfallForecast) });
            var checker = new SnowfallChecker(mock.Object);
            var snowfall = await checker.Check(new SubscriptionModel[] { _subscription });
            Assert.AreEqual(uriCount, snowfall.Count());
            mock.VerifyAll();
        }
    }
}
