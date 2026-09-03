using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainTimetable.Data.Migrations
{
    /// <inheritdoc />
    public partial class StationAndTrainChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Stations_BaseStationID",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_BaseStationID",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "BaseStationID",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "TrainNumber",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNode",
                table: "Stations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TrainNumber",
                table: "Trains",
                column: "TrainNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trains_TrainNumber",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TrainNumber",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "IsNode",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "BaseStationID",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_BaseStationID",
                table: "Stations",
                column: "BaseStationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Stations_BaseStationID",
                table: "Stations",
                column: "BaseStationID",
                principalTable: "Stations",
                principalColumn: "ID");
        }
    }
}
