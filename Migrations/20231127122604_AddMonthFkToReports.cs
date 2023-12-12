using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleCalcBot.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthFkToReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthID",
                table: "ReportsTable",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTable_MonthID",
                table: "ReportsTable",
                column: "MonthID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsTable_MonthsTable_MonthID",
                table: "ReportsTable",
                column: "MonthID",
                principalTable: "MonthsTable",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportsTable_MonthsTable_MonthID",
                table: "ReportsTable");

            migrationBuilder.DropIndex(
                name: "IX_ReportsTable_MonthID",
                table: "ReportsTable");

            migrationBuilder.DropColumn(
                name: "MonthID",
                table: "ReportsTable");
        }
    }
}
