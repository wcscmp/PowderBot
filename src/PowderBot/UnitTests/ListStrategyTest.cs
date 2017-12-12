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
            var subs = new SubscriptionModel[]{
                new SubscriptionModel(userId, "http://www.snow-forecast.com/resorts/Alta/6day/mid"),
                new SubscriptionModel(userId, "http://www.snow-forecast.com/resorts/Solitude/6day/mid")};
            var internalRepo = new MemoryRepository<SubscriptionModel>();
            await Task.WhenAll(subs.Select(s => internalRepo.InsertOrReplace(s)));
            var subscriptionRepo = new SubscriptionRepository(internalRepo);
            var (message, _) = await new ListStrategy(new UserModel(userId), subscriptionRepo)
                .Process();
            var messanger = new TestMessanger();
            await message.SendMessage(userId, messanger);
            Assert.IsTrue(subs.All(s => messanger.Text.Contains(s.GetResortName())));
        }
    }
}
