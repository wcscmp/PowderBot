using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("scheduler")]
    public class SchedulerController : Controller
    {
        public SchedulerController(FacebookClient facebookClient,
                                   SnowfallChecker snowfallChecker,
                                   UserRepository userRepo,
                                   SubscriptionRepository subscriptionRepo)
        {
            _facebookClient = facebookClient;
            _snowfallChecker = snowfallChecker;
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        private readonly FacebookClient _facebookClient;
        private readonly SnowfallChecker _snowfallChecker;
        private readonly UserRepository _userRepo;
        private readonly SubscriptionRepository _subscriptionRepo;

        [HttpPost]
        async public Task<IActionResult> Post()
        {
            var now = DateTimeOffset.UtcNow;
            var subscriptions = await _subscriptionRepo.GetOlderThen(now.AddMinutes(-23.5 * 60));
            var users = await _userRepo.GetAll();
            var snowfall = await _snowfallChecker.Check(users, subscriptions, now);
            var notifyTasks = snowfall.Select(s => notify(s.UserId, s.Subscriptions));
            var saveTasks = snowfall
                .SelectMany(s => _subscriptionRepo.CreateSaveTasks(s.Subscriptions));
            await Task.WhenAll(notifyTasks.Concat(saveTasks));
            return Ok();
        }

        private async Task notify(string userId, IEnumerable<SubscriptionModel> subs)
        {
            var uris = string.Join("\n", subs.Select(s => s.Uri));
            await _facebookClient.SendMessage(userId, "Check this out:\n" + uris);
        }
    }
}
