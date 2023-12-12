using iText.Kernel.Pdf;
using Microsoft.EntityFrameworkCore;
using PeopleCalcBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using static PeopleCalcBot.BotDBContext;

public class UpdateHandler
{
    private readonly DataRepository _dataRepository;
    private readonly BotDBContext _botDBContext;
    private readonly PdfController _pdfController;
    private const string templatePdf = "PeopleAmount_Form_Template.pdf";
    private const string documentName = "AmountReport.pdf";
    public UpdateHandler(DataRepository dataRepository, BotDBContext botDBContext, PdfController pdfController)
    {
        _dataRepository = dataRepository;
        _botDBContext = botDBContext;
        _pdfController = pdfController;
    }
    public UpdateHandler() : this(new DataRepository(), new BotDBContext(), new PdfController()) { }
    public enum BotState
    {
        None,
        WaitingForWeek,
        WaitingForAmount,
        WaitingForMonth,
        WaitingForMeetingType,
        WaitingForCongregation,
        WaitingForDistrict
    }
    public BotState CurrentState { get; set; }

    public MeetingTypes? meetingtype;
    public Congregations? congregarion;
    public Months? months;
    public BotDBContext.User? existUser;
    public Weeks? week;

    public async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var message = update.Message;
        if (message == null) { return; }


        try
        {
            switch (message.Text)
            {
                
                case "/enter_amount":


                    long? telegramId = message.From?.Id;
                    if (telegramId.HasValue && !await _dataRepository.IsUserByTelegramIdAsync(telegramId))
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Hello!      Hold on, I need to write your data to db",
                        cancellationToken: ct); var user = new BotDBContext.User()
                        {
                            Name = message.Chat.FirstName + " " + message.Chat.LastName,
                            TelegramID = message.Chat.Id,
                            CreatedDate = DateTime.Now
                        };

                        await _dataRepository.CreateUserAsync(user);
                        break;
                    }
                    else
                    {
                        await CreateExistUserInstanceAsync(message.From?.Id);

                        await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: @"What is your Congregation?   
                           Avalible:     Бабинці ",
                    cancellationToken: ct);
                        CurrentState = BotState.WaitingForCongregation;
                      
                        break;




                    }





                
                case "/get_PDF_overview":
                    if (await _dataRepository.IsDocumentExistAsync(message.From?.Id) != null)
                    {

                        var document = await _botDBContext.DocumentsTable.FirstOrDefaultAsync(d => d.User.TelegramID == message.From.Id);
                        using (FileStream fs = new FileStream($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PeopleAmount_Form.pdf")}", FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(document.File, 0, document.File.Length);
                        }

                   
                        using (var fileStream = new FileStream($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PeopleAmount_Form.pdf")}", FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var fileName = Path.GetFileName(fileStream.Name);
                            var inputFile = new InputFileStream(fileStream, fileName);

                            
                            await botClient.SendDocumentAsync(message.Chat.Id, inputFile);

                        }
                        System.IO.File.Delete($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PeopleAmount_Form.pdf")}");

                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "You can not get the file cause it is no exists.",
                        cancellationToken: ct);

                    }
                    break;

                default:

                    if (CurrentState == BotState.WaitingForCongregation)
                    {
                        congregarion = new Congregations()
                        {
                            ID = 1,
                            Name = message.Text,

                        };
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "What is Meeting Type?    " +
                        "/LifeAndMinistry     /WeekendMeeting",
                        cancellationToken: ct);
                        CurrentState = BotState.WaitingForMeetingType;
                        break;
                    }
                    else if (CurrentState == BotState.WaitingForMeetingType)
                    {
                        if (message.Text == "/LifeAndMinistry")
                        {
                            meetingtype = await _botDBContext.MeetingTypeTable.FirstOrDefaultAsync(m => m.Name == "LAM - Life and Ministry");

                            CurrentState = BotState.WaitingForMonth;
                            await botClient.SendTextMessageAsync(
                         chatId: message.Chat.Id,
                         text: "Enter month    /January /February  /March  /April  /May  /June" +
                         "  /July  /August  /September  /October  /November  /December ",
                         cancellationToken: ct);
                        }
                        else if (message.Text == "/WeekendMeeting")
                        {
                            meetingtype = await _botDBContext.MeetingTypeTable.FirstOrDefaultAsync(m => m.Name == "WKD - Weekend Meeting");

                            CurrentState = BotState.WaitingForMonth;
                            await botClient.SendTextMessageAsync(
                         chatId: message.Chat.Id,
                         text: "Enter month    /January /February  /March  /April  /May  /June" +
                         "  /July  /August  /September  /October  /November  /December ",
                         cancellationToken: ct);
                        }

                        else
                        {
                            await WrongAnswerMessageAsync(botClient, new Update(), new CancellationToken());
                        }
                    }
                    else if (CurrentState == BotState.WaitingForMonth)
                    {
                        switch (message.Text)
                        {
                            case "/January":
                                await CreateMonthInstanceAsync("January", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/February":
                                await CreateMonthInstanceAsync("February", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/March":
                                await CreateMonthInstanceAsync("March", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/April":
                                await CreateMonthInstanceAsync("April", botClient, new Update(), new CancellationToken(), message.Chat.Id);

                                break;
                            case "/May":
                                await CreateMonthInstanceAsync("May", botClient, new Update(), new CancellationToken(), message.Chat.Id);

                                break;

                            case "/June":
                                await CreateMonthInstanceAsync("June", botClient, new Update(), new CancellationToken(), message.Chat.Id);

                                break;

                            case "/July":
                                await CreateMonthInstanceAsync("July", botClient, new Update(), new CancellationToken(), message.Chat.Id);

                                break;

                            case "/August":
                                await CreateMonthInstanceAsync("August", botClient, new Update(), new CancellationToken(), message.Chat.Id);

                                break;

                            case "/September":
                                await CreateMonthInstanceAsync("September", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/October":
                                await CreateMonthInstanceAsync("October", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/November":
                                await CreateMonthInstanceAsync("November", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            case "/December":
                                await CreateMonthInstanceAsync("December", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;

                            default:

                                await WrongAnswerMessageAsync(botClient, new Update(), new CancellationToken());
                                break;
                        }


                    }
                    else if (CurrentState == BotState.WaitingForWeek)
                    {
                        switch (message.Text)
                        {
                            case "/1":
                                await CreateWeekInstanceAsync("First", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;
                            case "/2":
                                await CreateWeekInstanceAsync("Second", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;
                            case "/3":
                                await CreateWeekInstanceAsync("Third", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;
                            case "/4":
                                await CreateWeekInstanceAsync("Fourth", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;
                            case "/5":
                                await CreateWeekInstanceAsync("Fifth", botClient, new Update(), new CancellationToken(), message.Chat.Id);
                                break;
                            default:
                                await WrongAnswerMessageAsync(botClient, new Update(), new CancellationToken());

                                break;
                        }

                    }
                    else if (CurrentState == BotState.WaitingForAmount)
                    {
                        if (message.Text != null)
                        {

                            await _dataRepository.CreateReportAsync(message.Text, existUser, meetingtype, week, months, congregarion);
                            var existingDocument = await _dataRepository.IsDocumentExistAsync(message.From?.Id);
                            if (existingDocument != null)
                            {
                                var modifiedDocument = await _pdfController.ModifyPDFAsync(existingDocument.File, meetingtype?.Name, week?.Name, message.Text, congregarion, months);
                                await _dataRepository.UpdatePdfFieldsAsync(message.From?.Id, modifiedDocument);
                            }
                            else
                            {

                                var document = await _pdfController.FillPdfFieldsAsync(templatePdf, meetingtype?.Name, week?.Name, message.Text, congregarion, months);

                                await _dataRepository.UploadPdfToDbAsync(document, documentName, existUser, congregarion, months);

                            }

                            await _botDBContext.SaveChangesAsync();
                            await botClient.SendTextMessageAsync(
                     chatId: message.From.Id,
                     text: "Done!",
                     cancellationToken: ct);







                        }
                    }
                    break;


            }

        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"{ex.Message} {ex.InnerException}");
        }

    }
    
    public async Task CreateWeekInstanceAsync(string? weekName, ITelegramBotClient botClient, Update update, CancellationToken ct, long Id)
    {
        week = await _botDBContext.WeeksTable.FirstOrDefaultAsync(m => m.Name == weekName);
        CurrentState = BotState.WaitingForAmount;

        await botClient.SendTextMessageAsync(
                     chatId: Id,
                     text: "Enter amount of people: ",
                     cancellationToken: ct);
    }

    public async Task CreateMonthInstanceAsync(string? month, ITelegramBotClient botClient, Update update, CancellationToken ct, long Id)
    {
        months = await _botDBContext.MonthsTable.FirstOrDefaultAsync(m => m.Name == month);

        CurrentState = BotState.WaitingForWeek;
        await botClient.SendTextMessageAsync(
                     chatId: Id,
                     text: @"Enter week:   /1  /2  /3  /4  /5  ",
                     cancellationToken: ct);
    }
    public async Task CreateExistUserInstanceAsync(long? Id)
    {
        existUser = await _botDBContext.UsersTable.FirstOrDefaultAsync(m => m.TelegramID == Id);
    }
    public async Task WrongAnswerMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var message = update.Message ?? throw new NullReferenceException();

        await botClient.SendTextMessageAsync(
                     chatId: message.Chat.Id,
                     text: @"Wrong answer. Try again!",
                     cancellationToken: ct);
    }

}
