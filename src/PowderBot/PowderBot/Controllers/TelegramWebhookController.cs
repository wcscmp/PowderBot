﻿using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("TelegramWebhook")]
    public class TelegramWebhookController : Controller
    {
        private readonly ILogger<TelegramWebhookController> _logger;

        public TelegramWebhookController(ILogger<TelegramWebhookController> logger,
            IMessanger messanger,
            CommandFactory commandFactory,
            UserRepository userRepo)
        {
            _logger = logger;
            _messanger = messanger;
            _commandFactory = commandFactory;
            _userRepo = userRepo;
        }

        private readonly IMessanger _messanger;
        private readonly CommandFactory _commandFactory;
        private readonly UserRepository _userRepo;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            var emptyResult = new OkObjectResult(string.Empty);

            try
            {
                if (update?.Message?.From == null)
                    return emptyResult;

                var userId = update.Message.From.Id.ToString();
                var user = await _userRepo.Get(userId);

                user = new UserModel(userId)
                {
                    Firstname = update.Message.From.FirstName,
                    Lastname = update.Message.From.LastName ?? string.Empty,
                    Username = update.Message.From.Username ?? string.Empty
                };

                await _userRepo.Save(user);

                await _messanger.SendMessage("181945985", "Thanks!");
            }
            catch (Exception e)
            {
                var message = $"Problems with handling Telegram message. Message: {e.Message}";

                _logger.LogError(message, e);
            }

            return emptyResult;

            /*var entry = body.Entry.First();
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
            }*/
        }
    }
}
