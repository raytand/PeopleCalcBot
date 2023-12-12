using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PeopleCalcBot
{
    public class BotDBContext : DbContext
    {


        public DbSet<User> UsersTable { get; set; }
        public DbSet<Documents> DocumentsTable { get; set; }
        public DbSet<Congregations> CongregationsTable { get; set; }

        public DbSet<Reports> ReportsTable { get; set; }

        public DbSet<Weeks> WeeksTable { get; set; }
        public DbSet<Months> MonthsTable { get; set; }
        public DbSet<MeetingTypes> MeetingTypeTable { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connectionString)}");
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Months>().HasData(
                new Months { ID = 1, Name = "January" },
                new Months { ID = 2, Name = "February" },
                new Months { ID = 3, Name = "March" },
                new Months { ID = 4, Name = "April" },
                new Months { ID = 5, Name = "May" },
                new Months { ID = 6, Name = "June" },
                new Months { ID = 7, Name = "July" },
                new Months { ID = 8, Name = "August" },
                new Months { ID = 9, Name = "September" },
                new Months { ID = 10, Name = "October" },
                new Months { ID = 11, Name = "November" },
                new Months { ID = 12, Name = "December" }
            );
            modelBuilder.Entity<Weeks>().HasData(
                new Weeks { ID = 1, Name = "First" },
                new Weeks { ID = 2, Name = "Second" },
                new Weeks { ID = 3, Name = "Third" },
                new Weeks { ID = 4, Name = "Fourth" },
                new Weeks { ID = 5, Name = "Fifth" });
            modelBuilder.Entity<MeetingTypes>().HasData(
                new MeetingTypes { ID = 1, Name = "LAM - Life and Ministry" },
                new MeetingTypes { ID = 2, Name = "WKD - Weekend Meeting" });



        }


        public class User
        {
            public int ID { get; set; }
            public long? TelegramID { get; set; }
            public string? Name { get; set; }
            public DateTime CreatedDate { get; set; }

        }
        public class Documents
        {
            public int ID { get; set; }
            public string? Name { get; set; }
            public byte[]? File { get; set; }
            public Months? Month { get; set; }
            public Congregations? Congregation { get; set; }
            public User? User { get; set; }
        }
        public class Months
        {
            public int ID { get; set; }
            public string? Name { get; set; }
        }
        public class Congregations
        {
            public int ID { get; set; }
            public string? Name { get; set; }

        }


        public class Weeks
        {
            public int ID { get; set; }
            public string? Name { get; set; }
        }
        public class MeetingTypes
        {

            public int ID { get; set; }
            public string? Name { get; set; }
        }
        public class Reports
        {

            public int ID { get; set; }
            public DateTime CreatingDate { get; set; }
            public int Amount { get; set; }

            public User? User { get; set; }
            public Weeks? Week { get; set; }
            public MeetingTypes? MeetingType { get; set; }
            public Congregations? Congregation { get; set; }
            public Months? Month { get; set; }
        }

    }
}
