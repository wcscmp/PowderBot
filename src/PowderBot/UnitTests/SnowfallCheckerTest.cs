using BusinessLogic;
using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebClient;

namespace UnitTests
{
    [TestClass]
    public class SnowfallCheckerTest
    {
        private readonly UserModel _userWithoutSubscriptions = new UserModel("41");
        private readonly UserModel _user = new UserModel("42");
        private readonly SubscriptionModel _subscription
            = new SubscriptionModel("42", "http://www.snow-forecast.com/resorts/Alta/6day/mid")
            {
                Snowfall = 5
            };

        private readonly SubscriptionModel _invalidUserSubscription
            = new SubscriptionModel("43", "http://www.snow-forecast.com/resorts/Alta/6day/mid")
            {
                Snowfall = 5
            };

        [TestMethod]
        public async Task EmptyUserListIsAccepted()
        {
            var mock = new Mock<ISnowForecastClient>();
            mock
                .Setup(m => m.GetSnowfall(It.IsAny<IEnumerable<string>>()))
                .Callback<IEnumerable<string>>(uris => Assert.IsFalse(uris.Any()))
                .ReturnsAsync(new (string Uri, int Snowfall)[] { });
            var checker = new SnowfallChecker(mock.Object);
            var snowfall = await checker
                .Check(new UserModel[] { }, new SubscriptionModel[] { _subscription });
            Assert.IsFalse(snowfall.Any());
            mock.VerifyAll();
        }

        [TestMethod]
        public async Task EmptySubscriptionListIsAcceptasded()
        {
            var mock = new Mock<ISnowForecastClient>();
            mock
                .Setup(m => m.GetSnowfall(It.IsAny<IEnumerable<string>>()))
                .Callback<IEnumerable<string>>(uris => Assert.IsFalse(uris.Any()))
                .ReturnsAsync(new (string Uri, int Snowfall)[] { });
            var checker = new SnowfallChecker(mock.Object);
            var snowfall = await checker
                .Check(new UserModel[] { _user }, new SubscriptionModel[] { });
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
            var snowfall = await checker
                .Check(new UserModel[] { _user, _userWithoutSubscriptions },
                       new SubscriptionModel[] { _subscription, _invalidUserSubscription });
            Assert.AreEqual(uriCount, snowfall.Count());
            mock.VerifyAll();
        }
    }
}
