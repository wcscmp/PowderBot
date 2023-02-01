using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("Scheduler")]
    public class SchedulerController : Controller
    {
        public SchedulerController(IMessanger messanger,
                                   SnowfallChecker snowfallChecker,
                                   UserRepository userRepo,
                                   SubscriptionRepository subscriptionRepo)
        {
            _messanger = messanger;
            _snowfallChecker = snowfallChecker;
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        private readonly IMessanger _messanger;
        private readonly SnowfallChecker _snowfallChecker;
        private readonly UserRepository _userRepo;
        private readonly SubscriptionRepository _subscriptionRepo;

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var now = DateTimeOffset.UtcNow;
            var subscriptions = await _subscriptionRepo.GetAll();
            var users = await _userRepo.GetUsersWhoCanBeNotified(now);

            var subscriptionsForUsers = subscriptions
                .Join(users, s => s.ChatId, u => u.Id, (s, u) => (u, s))
                .Where(joined => !joined.Item2.UpdatedToday(joined.Item1, now))
                .Select(joined => joined.Item2);

            var snowfall = await _snowfallChecker.Check(subscriptionsForUsers);

            await Notify(snowfall);
            await SaveSubscriptions(snowfall, now);
            
            return Ok();
        }

        private async Task Notify(IEnumerable<SubscriptionModel> subs)
        {
            foreach (var subGroup in subs.GroupBy(s => s.ChatId))
            {
                await new MultiTextMessage(subGroup.Select(s => s.Uri), "Check this out:")
                    .SendMessage(subGroup.Key, _messanger);
            }
        }

        private async Task SaveSubscriptions(IEnumerable<SubscriptionModel> subs, DateTimeOffset now)
        {
            foreach (var sub in subs)
            {
                sub.LastMessageSent = now;
                await _subscriptionRepo.Save(sub);
            }
        }
    }
}
