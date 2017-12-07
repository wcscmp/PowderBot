using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PowderBot.Controllers
{
    [Route("debug")]
    public class DebugController : Controller
    {
        public DebugController(IGenericRepository<UserModel> userRepo,
                               IGenericRepository<SubscriptionModel> subscriptionRepo)
        {
            _userRepo = userRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        private readonly IGenericRepository<UserModel> _userRepo;
        private readonly IGenericRepository<SubscriptionModel> _subscriptionRepo;

        [HttpGet("{table}")]
        async public Task<IActionResult> Get(string table)
        {
            switch (table)
            {
            case "user":
                return Ok(await _userRepo.GetAll());
            case "subs":
                return Ok(await _subscriptionRepo.GetAll());
            }
            return Ok(table ?? "<null>");
        }
    }
}
