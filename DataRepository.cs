using Microsoft.EntityFrameworkCore;
using static PeopleCalcBot.BotDBContext;


namespace PeopleCalcBot
{
    public class DataRepository
    {
        private readonly BotDBContext _dbContext;
        public DataRepository(BotDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
        }
        public DataRepository() : this(new BotDBContext()) { }






        public async Task CreateUserAsync(BotDBContext.User user)
        {

            await _dbContext.UsersTable.AddRangeAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserByTelegramIdAsync(long? telegramId)
        {
            try
            {
                return await _dbContext.UsersTable
                   .AnyAsync(u => u.TelegramID == telegramId);
            }
            catch (Exception ex) { await Console.Out.WriteLineAsync(ex.Message); return false; }

        }
        public async Task<BotDBContext.Documents?> IsDocumentExistAsync(long? telegramId)
        {
            if (telegramId == null) { throw new ArgumentNullException(); }
            return await _dbContext.DocumentsTable.FirstOrDefaultAsync(u => u.User.TelegramID == telegramId);

        }
        public async Task CreateReportAsync(string amount, BotDBContext.User? user, BotDBContext.MeetingTypes? meetingType, BotDBContext.Weeks? week, BotDBContext.Months? month, BotDBContext.Congregations? congregation)
        {
            var existingCongregation = await _dbContext.CongregationsTable.FindAsync(congregation?.ID);
            var existingMeetingType = await _dbContext.MeetingTypeTable.FindAsync(meetingType?.ID);
            var existingWeek = await _dbContext.WeeksTable.FindAsync(week?.ID);
            var existingMonth = await _dbContext.MonthsTable.FindAsync(month?.ID);
            var existUser = await _dbContext.UsersTable.FindAsync(user?.ID);

            var report = new BotDBContext.Reports()
            {
                CreatingDate = DateTime.Now,
                Amount = Int32.Parse(amount),
                User = existUser ?? throw new ArgumentNullException(nameof(user)),
                MeetingType = existingMeetingType ?? throw new ArgumentNullException(nameof(meetingType)),
                Week = existingWeek ?? throw new ArgumentNullException(nameof(week)),
                Month = existingMonth ?? throw new ArgumentNullException(nameof(month)),
                Congregation = existingCongregation ?? throw new ArgumentNullException(nameof(congregation))
            };
            _dbContext.ReportsTable.Add(report);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UploadFileToDatabase(string filePath, string fileName, BotDBContext.User? user, BotDBContext.Congregations? congregation, BotDBContext.Months? month)
        {


            byte[] fileBytes;


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    fileBytes = br.ReadBytes((int)fs.Length);
                }
            }



            var existingCongregation = await _dbContext.CongregationsTable.FindAsync(congregation?.ID);
            var existingMonth = await _dbContext.MonthsTable.FindAsync(month?.ID);
            _dbContext.Entry(congregation).State = EntityState.Detached;
            var document = new BotDBContext.Documents()
            {
                File = fileBytes,
                Name = fileName,
                Congregation = existingCongregation,
                Month = existingMonth,
                User = user
            };
            _dbContext.DocumentsTable.Add(document);
            await _dbContext.SaveChangesAsync();

        }
        public async Task UploadPdfToDbAsync(byte[] fileBytes, string fileName, BotDBContext.User? user, BotDBContext.Congregations? congregation, BotDBContext.Months? month)
        {

            var existingCongregation = await _dbContext.CongregationsTable.FindAsync(congregation?.ID);
            var existingMonth = await _dbContext.MonthsTable.FindAsync(month?.ID);
            var existUser = await _dbContext.UsersTable.FindAsync(user?.ID);


            var pdfDocument = new Documents()
            {

                Name = fileName + $"_{congregation?.Name}" + $"_{DateTime.Now}",
                File = fileBytes,
                User = existUser,
                Congregation = existingCongregation,
                Month = existingMonth

            };

           
            await _dbContext.DocumentsTable.AddRangeAsync(pdfDocument);



            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdatePdfFieldsAsync(long? userId, byte[] modifiedPdfBytes)
        {

            var document = await _dbContext.DocumentsTable.FirstOrDefaultAsync(d => d.User.TelegramID == userId);

            if (document != null)
            {
                try
                {
                    document.File = modifiedPdfBytes; 
                    await _dbContext.SaveChangesAsync(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating document in the database: {ex.Message}");


                }
            }
            else
            {
                Console.WriteLine("Document not found in the database.");
            }

        }

    }
}
