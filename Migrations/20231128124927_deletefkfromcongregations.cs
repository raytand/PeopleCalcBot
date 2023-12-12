using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleCalcBot.Migrations
{
    /// <inheritdoc />
    public partial class Deletefkfromcongregations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CongregationsTable_DistrictsTable_DistrictID",
                table: "CongregationsTable");

            migrationBuilder.DropIndex(
                name: "IX_CongregationsTable_DistrictID",
                table: "CongregationsTable");

            migrationBuilder.DropColumn(
                name: "DistrictID",
                table: "CongregationsTable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictID",
                table: "CongregationsTable",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CongregationsTable_DistrictID",
                table: "CongregationsTable",
                column: "DistrictID");

            migrationBuilder.AddForeignKey(
                name: "FK_CongregationsTable_DistrictsTable_DistrictID",
                table: "CongregationsTable",
                column: "DistrictID",
                principalTable: "DistrictsTable",
                principalColumn: "ID");
        }
    }
}
