using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgetif.Model;
using budgetif.Model.Enitities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.CallbackProcessors
{
    public class AddDeputatCallback : CallbackProcessor
    {
        public override string Name => "add_deputat";

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
                    var deputat = context.Deputats.FirstOrDefault(p => p.Id == id);
                    if (deputat == null)
                    {
                        client.SendTextMessageAsync(user.TelegramId, "Депутата не знайдено");
                        return;
                    } else
                    {
                        user.Deputats.Add(deputat);
                        context.SaveChanges();
                        client.SendTextMessageAsync(user.TelegramId, $"Депутат {deputat.NameWithInitials} доданий до списку");
                        return;
                    }
                } else
                {
                    var text = "Такий депутат вже в списку";
                    client.SendTextMessageAsync(user.TelegramId, text);
                } 
            }
        }
    }
}
