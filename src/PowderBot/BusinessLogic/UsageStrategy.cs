using Data.Models;
using WebClient;

namespace BusinessLogic
{
    public class UsageStrategy : ICommandStrategy
    {
        private readonly UserModel _user;
        private readonly string[] usages = new string[] {
            CheckStrategy.Usage,
            SubscribeStrategy.Usage,
            ListStrategy.Usage,
            UnsubscribeStrategy.Usage};

        public UsageStrategy(UserModel user)
        {
            _user = user;
        }

        public Task<(IMessage, UserModel)> Process()
        {
            IMessage message = new MultiTextMessage(usages, "Usage:");
            return Task.FromResult((message, _user));
        }
    }
}
