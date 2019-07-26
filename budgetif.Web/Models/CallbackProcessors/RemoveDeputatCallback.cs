using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.CallbackProcessors
{
    public class RemoveDeputatCallback : CallbackProcessor
    {
        public override string Name => "remove_deputat";

        public override void Execute(IBudgetIfContext context, CallbackQuery query, TelegramBotClient client, ChatUser user)
        {
            var queryText = query.Data;
            var parts = queryText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var id = Convert.ToInt32(parts[1]);

                var exist = user.Deputats.FirstOrDefault(p => p.Id == id);
                if (exist == null)
                {

                    client.SendTextMessageAsync(user.TelegramId, "Депутата не знайдено");
                    return;
                }
                else
                {
                    user.Deputats.Remove(exist);
                    context.SaveChanges();
                    var text = $"Депутата {exist.NameWithInitials} видалено";
                    client.SendTextMessageAsync(user.TelegramId, text);
                }
            }
        }
    }
}
