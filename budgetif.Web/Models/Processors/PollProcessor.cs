using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgetif.Model;
using budgetif.Model.Enitities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.Processors
{
    public class PollProcessor : Processor
    {
        public override int Priority => 1;

        public override bool Process(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var items = context.Polls.Where(p => p.SubjectNo.Contains(message.Text)).ToList();
            if (items.Any())
            {
                var filtered = new List<Poll>();
                var first = items.Where(p => p.Subject.Contains(" - за основу")).ToList();
                if (first != null)
                {
                    filtered.AddRange(first);
                }
                var second = items.Where(p => p.Subject.Contains(" - у другому читанні та в цілому")).ToList();
                if (second != null)
                {
                    filtered.AddRange(second);
                }

                var list = user.Deputats.ToList();
                var text = "";
                foreach (var item in filtered)
                {
                    text += "<b>" + item.Subject.Trim() + "</b>\n";
                    text += "Час дата: " + item.VoteDate.ToString("yyyy-MM-dd HH:mm") + "\n";
                    text += (item.IsAccepted ? "<b>Рішення прийнято</b>" : "<b>Рішення провалено</b>") + "\n";
                    text += $"За: {item.Yes}\n Проти: {item.No}\n Утрималось: {item.Abstain}\n Не голосували: {item.NotVoted}\n";
                    text += "\n";
                    foreach (var deputat in list)
                    {
                        var vote = context.Votes.Where(p => p.PollId == item.Id && p.DeputatId == deputat.Id).FirstOrDefault();
                        if (vote != null)
                        {
                            text += $"<i>{deputat.NameWithInitials}</i> : {GetAnswer(vote.Answer)}" + "\n";
                        }
                    }
                    text += "\n\n";
                }
                client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
                return true;
            }
            return false;
        }


        private string GetAnswer(int answer)
        {
            switch (answer)
            {
                case 0:
                    return "Проти";
                case 1:
                    return "За";
                case 2:
                    return "Утримався";
                case 3:
                    return "Не голосував";
                case 4:
                    return "Відсутній";
            }
            return "хз";
        }
    }
}
