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
            const string chatId = "13";
            const string userId = "42";
            
            var subscription = new SubscriptionModel(chatId, "http://www.snow-forecast.com/resorts/Alta", userId);
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.InsertOrReplace(It.IsAny<SubscriptionModel>()))
                .Returns(Task.CompletedTask);
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            await new SubscribeStrategy(chatId,
                new UserModel(userId),
                new string[] { "/subscribe", subscription.Uri, "10cm" },
                subscriptionRepo)
                .Process();
            subscriptionRepoMock.VerifyAll();
        }

        [TestMethod]
        public async Task SnowfallMayBeSpecifiedInInches()
        {
            const string chatId = "13";
            const string userId = "42";

            var subscription = new SubscriptionModel(chatId, "http://www.snow-forecast.com/resorts/Alta", userId);
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.InsertOrReplace(It.IsAny<SubscriptionModel>()))
                .Returns(Task.CompletedTask);
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            await new SubscribeStrategy(chatId,
                new UserModel(userId),
                new string[] { "/subscribe", subscription.Uri, "10inch" },
                subscriptionRepo)
                .Process();
            subscriptionRepoMock.VerifyAll();
        }

        [TestMethod]
        public async Task SnowfallMayNotBeSpecifiedInInvalidUnits()
        {
            const string chatId = "13";
            const string userId = "42";

            var subscription = new SubscriptionModel(chatId, "http://www.snow-forecast.com/resorts/Alta", userId);
            var subscriptionRepo = new SubscriptionRepository(null);
            await new SubscribeStrategy(chatId,
                new UserModel(userId),
                new string[] { "/subscribe", subscription.Uri, "10asd" },
                subscriptionRepo)
                .Process();
        }
    }
}
