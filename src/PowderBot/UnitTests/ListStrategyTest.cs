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
            var (message, _) = await new ListStrategy(new UserModel(userId), subscriptionRepo)
                .Process();
            var messanger = new TestMessanger();
            await message.SendMessage(messanger);
            Assert.IsTrue(uris.All(uri => messanger.Text.Contains(uri)));
        }
    }
}
