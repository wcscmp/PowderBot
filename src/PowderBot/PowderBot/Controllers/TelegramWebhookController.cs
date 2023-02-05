﻿using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using WebClient;

namespace PowderBot.Controllers
{
    [Route("TelegramWebhook")]
    public class TelegramWebhookController : Controller
    {
        private readonly ILogger<TelegramWebhookController> _logger;
        private readonly TelegramConfiguration _telegramConfiguration;

        public TelegramWebhookController(
            ILogger<TelegramWebhookController> logger,
            IOptions<TelegramConfiguration> telegramConfiguration,
            IMessanger messanger,
            CommandFactory commandFactory,
            UserRepository userRepo)
        {
            _logger = logger;
            _telegramConfiguration = telegramConfiguration.Value;
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
            if (!Request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var tokenHeader) ||
                tokenHeader.FirstOrDefault() != _telegramConfiguration.SecretToken)
                return Ok();

            try
            {
                if (update?.Message?.From == null ||
                    update?.Message?.Chat == null ||
                    string.IsNullOrWhiteSpace(update?.Message?.Text))
                    return Ok();

                var userId = update.Message.From.Id.ToString();
                var chatId = update.Message.Chat.Id.ToString();

                var user = (await _userRepo.Get(userId)) ?? new UserModel(userId);
                user.Firstname = update.Message.From.FirstName;
                user.Lastname = update.Message.From.LastName ?? string.Empty;
                user.Username = update.Message.From.Username ?? string.Empty;

                await _userRepo.Save(user);

                var (response, updatedUser) = await _commandFactory
                    .Create(chatId, user, update.Message.Text)
                    .Process();

                await response.SendMessage(chatId, _messanger);

                updatedUser.Gmt = await _messanger.QueryUserTimezone(updatedUser.Id);

                await _userRepo.Save(updatedUser);
            }
            catch (Exception e)
            {
                var message = $"Problems with handling Telegram message. Message: {e.Message}";

                _logger.LogError(message, e);
            }

            return Ok();
        }
    }
}
