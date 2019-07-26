using budgetif.Web.Models.CallbackProcessors;
using budgetif.Web.Models.Commands;
using budgetif.Web.Models.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace budgetif.Web.Models
{
    public static class Bot
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static TelegramBotClient client;

        private static List<Command> commandsList;

        private static List<CallbackProcessor> callbacksList;

        private static List<Processor> processorList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static IReadOnlyList<CallbackProcessor> Callbacks => callbacksList.AsReadOnly();


        public static IReadOnlyList<Processor> Processors => processorList.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            logger.Debug("Get bot");
            if (client != null)
            {
                return client;
            }

            commandsList = new List<Command>();
            commandsList.Add(new ListCommand());
            commandsList.Add(new RemoveCommand());
            commandsList.Add(new TopCommand());
            commandsList.Add(new StartCommand());

            callbacksList = new List<CallbackProcessor>();
            callbacksList.Add(new AddDeputatCallback());
            callbacksList.Add(new RemoveDeputatCallback());

            processorList = new List<Processor>();
            processorList.Add(new DeputatProcessor());
            processorList.Add(new PollProcessor());


            //TODO: Add more commands

            client = new TelegramBotClient(AppSettings.Key);
            var hook = string.Format(AppSettings.Url, "api/message/update");
            logger.Debug("HOOK " + hook);

            await client.SetWebhookAsync(hook);

            logger.Debug("Client return");
            return client;
        }
    }
}