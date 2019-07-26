using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace budgetif.Web.Models.Commands
{
    public class RemoveCommand : Command
    {
        public override string Name => "/remove";

        public override void Execute(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;

            var list = user.Deputats.ToList();
            var keys = list.Select(p => new InlineKeyboardCallbackButton(p.NameWithInitials, $"remove_deputat {p.Id}")).ToList();

            var matrix = new List<InlineKeyboardButton[]>();
            var offset = 0;
            List<InlineKeyboardButton> subList = new List<InlineKeyboardButton>();
            do
            {
                subList = keys.Skip(offset).Take(3).Select(p => (InlineKeyboardButton)p).ToList();
                offset += 3;
                matrix.Add(subList.ToArray());
            } while (subList.Count > 0);
            
            var replyMarkup = new InlineKeyboardMarkup(matrix.ToArray());

            var text = "Виберіть депутата якого видалити:";
            client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, replyMarkup);
        }
    }
}
