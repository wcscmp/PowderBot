using BusinessLogic;
using Data;
using Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class ListStrategyTest
    {
        [TestMethod]
        public async Task UrisAreListed()
        {
            const string userId = "42";
            var uris = new string[]{"http://www.snow-forecast.com/resorts/Alta/6day/mid",
                                    "http://www.snow-forecast.com/resorts/Solitude/6day/mid"};
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.GetAll())
                .ReturnsAsync(uris.Select(uri => new SubscriptionModel(userId, uri)));
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            var (message, _) = await new ListStrategy(subscriptionRepo)
                .Process(new UserModel(userId));
            Assert.IsTrue(uris.All(uri => message.Contains(uri)));
        }

        [TestMethod]
        public async Task NotificationTimeRangeIsDisplayed()
        {
            var user = new UserModel("42")
            {
                NotifyAfter = 10,
                NotifyBefore = 20
            };
            var subscriptionRepoMock = new Mock<IGenericRepository<SubscriptionModel>>();
            subscriptionRepoMock
                .Setup(mock => mock.GetAll())
                .ReturnsAsync(new SubscriptionModel[0] { });
            var subscriptionRepo = new SubscriptionRepository(subscriptionRepoMock.Object);
            var (message, _) = await new ListStrategy(subscriptionRepo).Process(user);
            Assert.IsTrue(message.Contains(user.NotifyAfter.ToString()));
            Assert.IsTrue(message.Contains(user.NotifyBefore.ToString()));
        }
    }
}
