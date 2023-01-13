using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class SubscribeStrategyTest
    {
        [TestMethod]
        public async Task SnowfallMayBeSpecifiedInCm()
        {
            const string userId = "42";
            var subscription = new SubscriptionModel(userId,
                                                     "http://www.snow-forecast.com/resorts/Alta");
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.InsertOrReplace(It.IsAny<SubscriptionModel>()))
                .Returns(Task.CompletedTask);
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            await new SubscribeStrategy(new UserModel(userId),
                                        new string[] { "/subscribe", subscription.Uri, "10cm" },
                                        subscriptionRepo)
                .Process();
            subscriptionRepoMock.VerifyAll();
        }

        [TestMethod]
        public async Task SnowfallMayBeSpecifiedInInches()
        {
            const string userId = "42";
            var subscription = new SubscriptionModel(userId,
                                                     "http://www.snow-forecast.com/resorts/Alta");
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.InsertOrReplace(It.IsAny<SubscriptionModel>()))
                .Returns(Task.CompletedTask);
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            await new SubscribeStrategy(new UserModel(userId),
                                        new string[] { "/subscribe", subscription.Uri, "10inch" },
                                        subscriptionRepo)
                .Process();
            subscriptionRepoMock.VerifyAll();
        }

        [TestMethod]
        public async Task SnowfallMayNotBeSpecifiedInInvalidUnits()
        {
            const string userId = "42";
            var subscription = new SubscriptionModel(userId,
                                                     "http://www.snow-forecast.com/resorts/Alta");
            var subscriptionRepo = new SubscriptionRepository(null);
            await new SubscribeStrategy(new UserModel(userId),
                                        new string[] { "/subscribe", subscription.Uri, "10asd" },
                                        subscriptionRepo)
                .Process();
        }
    }
}
