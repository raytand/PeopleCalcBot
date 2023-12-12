using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleCalcBot.Migrations
{
    /// <inheritdoc />
    public partial class FixMeetingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MeetingTypeTable",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "WKD - Weekend Meeting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MeetingTypeTable",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "WKD - Weekend Meenting");
        }
    }
}
