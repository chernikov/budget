using budgetif.Model;
using budgetif.Model.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace budgetif.Web.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract void Execute(IBudgetIfContext context, Message message, TelegramBotClient client, ChatUser user);

        public bool Contains(string command)
        {
            var subCommand = command.Substring(1);
            return Name == subCommand;
        }

    }
}