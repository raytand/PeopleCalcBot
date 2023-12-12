
using System;
using Telegram.Bot;

namespace PeopleCalcBot
{
    public class ErrorHandler
    {
        public ErrorHandler() { }
        public async Task ErrorHandlerAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));

        }
    }
}
