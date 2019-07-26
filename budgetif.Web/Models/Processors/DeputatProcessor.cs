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

namespace budgetif.Web.Models.Processors
{
    public class DeputatProcessor : Processor
    {
        public override int Priority => 3;

        public override bool Process(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var text = string.Empty;

            var list = context.Deputats.ToList().Where(p => p.NameWithInitials.ToLower().Contains(message.Text)).ToList();
            if (list.Count() == 1)
            {
                text = $"Додати депутата {list[0].NameWithInitials} в мій список?";
                var replyMarkup = new InlineKeyboardMarkup(new InlineKeyboardCallbackButton[]
                {
                        new InlineKeyboardCallbackButton("Так", "add_deputat " + list[0].Id),
                        new InlineKeyboardCallbackButton("Ні", "cancel")
                });
                client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, replyMarkup);
                return true;
            }
            else if (list.Count() > 1)
            {
                text = "Виберіть депутата якого додати в список";

                var keys = list.Select(p => new InlineKeyboardCallbackButton(p.NameWithInitials, $"add_deputat {p.Id}")).ToList();
                var replyMarkup = new InlineKeyboardMarkup(keys.ToArray());
                client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, replyMarkup);
                return true;
            }
            return false;
        }
    }
}
