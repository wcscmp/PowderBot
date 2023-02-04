using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("Scheduler")]
    public class SchedulerController : Controller
    {
        private readonly SchedulerConfiguration _schedulerConfiguration;
        private readonly IMessanger _messanger;
        private readonly SnowfallChecker _snowfallChecker;
        private readonly UserRepository _userRepo;
        private readonly SubscriptionRepository _subscriptionRepo;

        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(
            IOptions<SchedulerConfiguration> schedulerConfiguration,
            IMessanger messanger,
            SnowfallChecker snowfallChecker,
            UserRepository userRepo,
            SubscriptionRepository subscriptionRepo,
            ILogger<SchedulerController> logger)
        {
            _schedulerConfiguration = schedulerConfiguration.Value;
            _messanger = messanger;
            _snowfallChecker = snowfallChecker;
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;

            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            _logger.LogCritical($"SecretToken: {_schedulerConfiguration.SecretToken}");

            if (!Request.Headers.TryGetValue("X-Scheduler-Secret-Token", out var tokenHeader) ||
                tokenHeader.FirstOrDefault() != _schedulerConfiguration.SecretToken)
            {
                _logger.LogCritical($"Empty TokenHeader: {tokenHeader}");
            }
            else
            {
                _logger.LogCritical($"TokenHeader: {tokenHeader}");
            }

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
