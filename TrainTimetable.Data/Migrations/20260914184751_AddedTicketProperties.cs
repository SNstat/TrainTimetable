using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainTimetable.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTicketProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TicketSchedules_LineScheduleID",
                table: "TicketSchedules",
                column: "LineScheduleID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketSchedules_LineScheduleID",
                table: "TicketSchedules");
        }
    }
}
