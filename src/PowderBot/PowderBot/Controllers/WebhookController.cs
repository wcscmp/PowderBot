using BusinessLogic;
using Data;
using Microsoft.AspNetCore.Mvc;
using PowderBot.ApiTypes.Facebook;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("webhook")]
    public class WebhookController : Controller
    {
        public WebhookController(IMessanger messanger,
                                 CommandFactory commandFactory,
                                 UserRepository userRepo)
        {
            _messanger = messanger;
            _commandFactory = commandFactory;
            _userRepo = userRepo;
        }

        private readonly IMessanger _messanger;
        private readonly CommandFactory _commandFactory;
        private readonly UserRepository _userRepo;

        [HttpGet]
        public IActionResult Get()
        {
            const string verifyToken = "b940e968-d34c-11e7-9296-cec278b6b50a";
            if (Request.Query["hub.mode"] != "subscribe"
                || Request.Query["hub.verify_token"] != verifyToken)
            {
                return Forbid();
            }
            return Ok(Request.Query["hub.challenge"].First());
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromBody]Event body)
        {
            if (body.Object != null && body.Object != "page")
            {
                return NotFound();
            }
            var entry = body.Entry.First();
            var user = await _userRepo.Get(entry.Messaging.First().Sender.Id);
            try
            {
                var message = entry.Messaging.First().Message;
                var messageText = message.QuickReply?.Payload ?? message.Text;
                var (response, updatedUser) = await _commandFactory
                    .Create(user, messageText)
                    .Process();
                await response.SendMessage(updatedUser.Id, _messanger);
                updatedUser.Gmt = await _messanger.QueryUserTimezone(updatedUser.Id);
                await _userRepo.Save(updatedUser);
                return Ok();
            }
            catch (Exception)
            {
                await new WebClient.TextMessage("Sorry, something went wrong")
                    .SendMessage(user.Id, _messanger);
                return Ok();
            }
        }
    }
}
