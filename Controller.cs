using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace PeopleCalcBot
{
    public class Controller
    {
        private UpdateHandler _updateHandler;
        private ErrorHandler _errorHandler;
        public Controller() : this(new UpdateHandler(), new ErrorHandler()) { }
        public Controller(UpdateHandler updateHandler, ErrorHandler errorHandler)
        {
            _updateHandler = updateHandler;
            _errorHandler = errorHandler;
        }
        public async Task ControllerAsync()
        {
            //TOKEN
            var client = new TelegramBotClient("6880221677:AAGWYvXXsTlxVKeJAlfT7Q1wT0_f78JJqQw");
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            await Task.Run(() =>
            {
                client.StartReceiving(
                _updateHandler.UpdateHandlerAsync,
                _errorHandler.ErrorHandlerAsync,
                receiverOptions,
                cancellationToken);
            });
            



            Console.ReadLine();
        }
    }
}
