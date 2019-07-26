using budgetif.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.Commands
{
    //public class KikoCommand : Command
    //{

    //    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    //    public override string Name => "kiko";

    //    public override void Execute(Message message, TelegramBotClient client)
    //    {
    //        var chatId = message.Chat.Id;
    //        var messageId = message.MessageId;
    //        var db = new BudgetIfContext();

    //        var total = db.Transactions.Sum(p => p.amount);
    //        var text = $"За серпень 2017 року витратили {total:#.00} гривень... \n\r/kikokiko";
    //        client.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
    //    }
    //}
}