using budgetif.Model;
using budgetif.Model.Enitities;
using budgetif.Web.Models;
using budgetif.Web.Models.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;

namespace budgetif.Web.Controllers
{
    public class MessageController : BaseApiController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [Route(@"api/message/update")] //webhook uri part
        public async Task<OkResult> Post([FromBody]Update update)
        {
            logger.Debug(JsonConvert.SerializeObject(update));
            var result = false;
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQueryUpdate)
            {
                var query = update.CallbackQuery;
                var user = InitUser(update.CallbackQuery.From);
                var callbacks = Bot.Callbacks;
                var client = await Bot.Get();
                foreach (var callback in callbacks)
                {
                    if (query.Data.StartsWith(callback.Name))
                    {
                        callback.Execute(Context, query, client, user);
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                var user = InitUser(update.Message.Chat);
                var commands = Bot.Commands;
                var message = update.Message;
                var client = await Bot.Get();
                foreach (var command in commands)
                {
                    if (message.Text.StartsWith(command.Name))
                    {
                        command.Execute(Context, message, client, user);
                        result = true;
                        break;
                    }
                }
                if (!result)
                {
                    foreach(var processor in Bot.Processors.OrderBy(p => p.Priority))
                    {
                        if (processor.Process(Context, message, client, user))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            //if (!result)
            //{
            //    var client = await Bot.Get();
            //    await client.SendTextMessageAsync(update.Message.Chat.Id, $"Нічого не поняв на оце: ({update.Message.Text})");
            //}

            return Ok();
        }

        public ChatUser InitUser(Chat chat)
        {
            var user = Context.ChatUsers.FirstOrDefault(p => p.TelegramId == chat.Id);
            if (user == null)
            {
                user = new ChatUser()
                {
                    TelegramId = chat.Id,
                    FirstName = chat.FirstName,
                    LastName = chat.LastName
                };
                Context.ChatUsers.Add(user);
                Context.SaveChanges();
                AddRandomDeputats(user);
            }
            return user;
        }

        public ChatUser InitUser(User chat)
        {
            var user = Context.ChatUsers.FirstOrDefault(p => p.TelegramId == chat.Id);
            if (user == null)
            {
                user = new ChatUser()
                {
                    TelegramId = chat.Id,
                    FirstName = chat.FirstName,
                    LastName = chat.LastName
                };
                Context.ChatUsers.Add(user);
                Context.SaveChanges();
                AddRandomDeputats(user);
            }
            return user;
        }

        private void AddRandomDeputats(ChatUser user)
        {
            var deputats = Context.Deputats.OrderBy(p => Guid.NewGuid()).Take(3).ToList();
            foreach(var deputat in deputats)
            {
                user.Deputats.Add(deputat);
            }
            Context.SaveChanges();
        }
    }
}