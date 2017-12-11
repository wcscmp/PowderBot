using BusinessLogic;
using Data;
using Data.Models;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class NotifyStrategyTest
    {
        [TestMethod]
        public async Task NotifyAfterIsSet()
        {
            const string userId = "42";
            var words = new string[] { "notify", "after", "9" };
            var (_, user) = await new NotifyStrategy(words)
                .Process(new UserModel(userId));
            Assert.AreEqual(words[2], user.NotifyAfter.ToString());
        }

        [TestMethod]
        public async Task NotifyBeforeIsSet()
        {
            const string userId = "42";
            var words = new string[] { "notify", "before", "9" };
            var (_, user) = await new NotifyStrategy(words)
                .Process(new UserModel(userId));
            Assert.AreEqual(words[2], user.NotifyBefore.ToString());
        }

        [TestMethod]
        public async Task InvalidNotifyAfterIsNotSet()
        {
            const string userId = "42";
            var words = new string[] { "notify", "after", "29" };
            var (_, user) = await new NotifyStrategy(words)
                .Process(new UserModel(userId));
            Assert.AreNotEqual(words[2], user.NotifyAfter.ToString());
        }

        [TestMethod]
        public async Task InvalidNotifyBeforeIsNotSet()
        {
            const string userId = "42";
            var words = new string[] { "notify", "after", "-9" };
            var (_, user) = await new NotifyStrategy(words)
                .Process(new UserModel(userId));
            Assert.AreNotEqual(words[2], user.NotifyAfter.ToString());
        }
    }
}
