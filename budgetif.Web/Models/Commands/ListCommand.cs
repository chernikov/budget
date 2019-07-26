using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgetif.Model;
using budgetif.Model.Enitities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace budgetif.Web.Models.Commands
{
    public class ListCommand : Command
    {
        public override string Name => "/list";

        public override void Execute(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var text = string.Join("\n", user.Deputats.Select(p => p.NameWithInitials));
            client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Default);
            return;
        }
    }
}
