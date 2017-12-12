using BusinessLogic;
using Data;
using Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var internalRepo = new MemoryRepository<SubscriptionModel>();
            await Task.WhenAll(uris.Select(uri =>
                internalRepo.InsertOrReplace(new SubscriptionModel(userId, uri))));
            var subscriptionRepo = new SubscriptionRepository(internalRepo);
            var (message, _) = await new ListStrategy(new UserModel(userId), subscriptionRepo)
                .Process();
            var messanger = new TestMessanger();
            await message.SendMessage(userId, messanger);
            Assert.IsTrue(uris.All(uri => messanger.Text.Contains(uri)));
        }
    }
}
