using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgetif.Model;
using budgetif.Model.Enitities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override void Execute(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;
            var text = "Тут все просто. Наберіть прізвище депутата (наприклад Шевченко), і додайте його собі в список (ми вже додали пару випадкових)." +
                "\n Перегляд списку за командою /list. Для того, щоб видалити команда скористайтесь командою /remove. " +
                "Для перегляду голосування наберіть номер законопроекту, наприклад, 6327 - номер законопроекту реформи медичної системи. Бот покаже голосування за перше і друге читання вибраних депутатів. " +
                "Наберіть /top і бот виведе 20 найобговорюваніших законопроектів 2017 року.";
            client.SendTextMessageAsync(chatId, text);
        }
    }
}
