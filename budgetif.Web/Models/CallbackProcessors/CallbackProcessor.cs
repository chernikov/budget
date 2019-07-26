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
    public abstract class CallbackProcessor
    {
        public abstract string Name { get; }

        public abstract void Execute(IBudgetIfContext context, CallbackQuery query, TelegramBotClient client, ChatUser user);

        public bool Contains(string command)
        {
            var subCommand = command.Substring(1);
            return Name == subCommand;
        }
    }
}
