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
    public class TopCommand : Command
    {
        public override string Name => "/top";

        public override void Execute(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var authority = context.Authorities.First();
            var pollList = authority.PollLists.First();
            var list = pollList.PollBatches.ToList();
            var opinions = authority.Opinions.ToList();
            var listDeputats = user.Deputats.ToList();

            var parts = message.Text.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var id = Convert.ToInt32(parts[1]);
                var batch = list.FirstOrDefault(p => p.Id == id);
                if (batch != null)
                {
                    var opinion = opinions.FirstOrDefault(p => p.PollBatchId == batch.Id);
                    var text = $"/top_{batch.Id} {RemoveBlaBla(batch.Name).Trim()} \n<b>{batch.Status.ToUpper().Trim()}</b> " + "\n";
                    if (opinion != null)
                    {
                        text += $"<b>{opinion.Description}</b>\n";
                    }
                    
                    var first = batch.Polls.OrderByDescending(p => p.VoteDate).FirstOrDefault(p => p.Subject.Contains(" - за основу"));
                    if (first != null)
                    {
                        text += "<i>Перше читання:</i>\n";
                        text += "Дата: " + first.VoteDate.ToString("yyyy-MM-dd HH:mm") + "\n";
                        foreach (var deputat in listDeputats)
                        {
                            var vote = context.Votes.Where(p => p.PollId == first.Id && p.DeputatId == deputat.Id).FirstOrDefault();
                            if (vote != null)
                            {
                                text += $"<i>{deputat.NameWithInitials}</i> : {GetAnswer(vote.Answer)} {(opinion != null ? "<b>(" + ZradaPeremoga(opinion.IsSupport > 0, vote.Answer) + ")</b>" : "")}" + "\n";
                            }
                        }
                        text += "\n";
                    }
                    var second = batch.Polls.OrderByDescending(p => p.VoteDate).FirstOrDefault(p => p.Subject.Contains(" - у другому читанні та в цілому"));
                    if (second != null)
                    {
                        text += "<i>Друге читання:</i>\n";
                        text += "Дата: " + second.VoteDate.ToString("yyyy-MM-dd HH:mm") + "\n";
                        foreach (var deputat in listDeputats)
                        {
                            var vote = context.Votes.Where(p => p.PollId == first.Id && p.DeputatId == deputat.Id).FirstOrDefault();
                            if (vote != null)
                            {
                                text += $"<i>{deputat.NameWithInitials}</i> : {GetAnswer(vote.Answer)} {(opinion != null ? "<b>(" + ZradaPeremoga(opinion.IsSupport > 0, vote.Answer)+ ")</b>" : "")}" + "\n";
                            }
                        }
                        text += "\n";
                    }
                    client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
                } else
                {
                    client.SendTextMessageAsync(chatId, "Нічого не знайдено");
                }
            }
            else
            {
                var text = "";
                foreach (var batch in list)
                {
                    var opinion = opinions.FirstOrDefault(p => p.PollBatchId == batch.Id);
                    if (opinion != null)
                    {
                        text += $"<b>{opinion.Description.ToUpper().Trim()}</b> <i>({(opinion.IsSupport > 0 ? "Перемога" : "Зрада")})</i>\n";
                    }
                    text += $"/top_{batch.Id} {RemoveBlaBla(batch.Name).Trim()} \n<b>{batch.Status.Trim()}</b> " + "\n\n";
                }
                client.SendTextMessageAsync(chatId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
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

        private string ZradaPeremoga(bool isSupport, int answer)
        {
            if (isSupport)
            {
                return answer == 1 ? "Перемога" : "Зрада";

            } else
            {
                return answer != 1 ? "Перемога" : "Зрада";
            }
        }
    

        private string RemoveBlaBla(string input)
        {
            return input.Replace("Поіменне голосування  про проект ", "");
        }
    }
}
