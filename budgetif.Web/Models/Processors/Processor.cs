using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.Processors
{
    public abstract class Processor
    {
        public abstract int Priority { get; }

        public abstract bool Process(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user);
    }
}
