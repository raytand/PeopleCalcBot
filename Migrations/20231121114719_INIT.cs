using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PeopleCalcBot.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistrictsTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictsTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MeetingTypeTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingTypeTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MonthsTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthsTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UsersTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TelegramID = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeeksTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeksTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CongregationsTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongregationsTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CongregationsTable_DistrictsTable_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "DistrictsTable",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReportsTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: true),
                    WeekID = table.Column<int>(type: "INTEGER", nullable: true),
                    MeetingTypeID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReportsTable_MeetingTypeTable_MeetingTypeID",
                        column: x => x.MeetingTypeID,
                        principalTable: "MeetingTypeTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReportsTable_UsersTable_UserID",
                        column: x => x.UserID,
                        principalTable: "UsersTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReportsTable_WeeksTable_WeekID",
                        column: x => x.WeekID,
                        principalTable: "WeeksTable",
                        principalColumn: "ID");
                    
                });

            migrationBuilder.CreateTable(
                name: "DocumentsTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    File = table.Column<byte[]>(type: "BLOB", nullable: true),
                    MonthID = table.Column<int>(type: "INTEGER", nullable: true),
                    CongregationID = table.Column<int>(type: "INTEGER", nullable: true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocumentsTable_CongregationsTable_CongregationID",
                        column: x => x.CongregationID,
                        principalTable: "CongregationsTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DocumentsTable_MonthsTable_MonthID",
                        column: x => x.MonthID,
                        principalTable: "MonthsTable",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DocumentsTable_UsersTable_UserID",
                        column: x => x.UserID,
                        principalTable: "UsersTable",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "MeetingTypeTable",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "LAM - Life and Ministry" },
                    { 2, "WKD - Weekend Meenting" }
                });

            migrationBuilder.InsertData(
                table: "MonthsTable",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "January" },
                    { 2, "February" },
                    { 3, "March" },
                    { 4, "April" },
                    { 5, "May" },
                    { 6, "June" },
                    { 7, "July" },
                    { 8, "August" },
                    { 9, "September" },
                    { 10, "October" },
                    { 11, "November" },
                    { 12, "December" }
                });

            migrationBuilder.InsertData(
                table: "WeeksTable",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "First" },
                    { 2, "Second" },
                    { 3, "Third" },
                    { 4, "Fourth" },
                    { 5, "Fifth" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CongregationsTable_DistrictID",
                table: "CongregationsTable",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsTable_CongregationID",
                table: "DocumentsTable",
                column: "CongregationID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsTable_MonthID",
                table: "DocumentsTable",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsTable_UserID",
                table: "DocumentsTable",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTable_MeetingTypeID",
                table: "ReportsTable",
                column: "MeetingTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTable_UserID",
                table: "ReportsTable",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTable_WeekID",
                table: "ReportsTable",
                column: "WeekID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentsTable");

            migrationBuilder.DropTable(
                name: "ReportsTable");

            migrationBuilder.DropTable(
                name: "CongregationsTable");

            migrationBuilder.DropTable(
                name: "MonthsTable");

            migrationBuilder.DropTable(
                name: "MeetingTypeTable");

            migrationBuilder.DropTable(
                name: "UsersTable");

            migrationBuilder.DropTable(
                name: "WeeksTable");

            migrationBuilder.DropTable(
                name: "DistrictsTable");
        }
    }
}
