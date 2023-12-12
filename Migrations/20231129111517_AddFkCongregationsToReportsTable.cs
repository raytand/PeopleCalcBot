using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleCalcBot.Migrations
{
    /// <inheritdoc />
    public partial class AddFkCongregationsToReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CongregationID",
                table: "ReportsTable",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportsTable_CongregationID",
                table: "ReportsTable",
                column: "CongregationID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsTable_CongregationsTable_CongregationID",
                table: "ReportsTable",
                column: "CongregationID",
                principalTable: "CongregationsTable",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportsTable_CongregationsTable_CongregationID",
                table: "ReportsTable");

            migrationBuilder.DropIndex(
                name: "IX_ReportsTable_CongregationID",
                table: "ReportsTable");

            migrationBuilder.DropColumn(
                name: "CongregationID",
                table: "ReportsTable");
        }
    }
}
