using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainTimetable.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TrainManufacturer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainManufacturer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseStationID = table.Column<int>(type: "int", nullable: true),
                    CountryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stations_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stations_Stations_BaseStationID",
                        column: x => x.BaseStationID,
                        principalTable: "Stations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeatCount = table.Column<int>(type: "int", nullable: false),
                    TrainManufacturerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Trains_TrainManufacturer_TrainManufacturerID",
                        column: x => x.TrainManufacturerID,
                        principalTable: "TrainManufacturer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    TrainID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lines_Trains_TrainID",
                        column: x => x.TrainID,
                        principalTable: "Trains",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    LineID = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    DepartureTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stops_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stops_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Croatia" });

            migrationBuilder.InsertData(
                table: "TrainManufacturer",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "FS Trenitalia" },
                    { 2, "Bombardier Transportation" },
                    { 3, "Alstom" }
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "ID", "BaseStationID", "CountryID", "Name" },
                values: new object[] { 1, null, 1, "Varaždin" });

            migrationBuilder.InsertData(
                table: "Trains",
                columns: new[] { "ID", "Name", "SeatCount", "TrainManufacturerID" },
                values: new object[,]
                {
                    { 1, "E.403", 60, 1 },
                    { 2, "S Stock", 50, 2 },
                    { 3, "X65", 76, 3 }
                });

            migrationBuilder.InsertData(
                table: "Lines",
                columns: new[] { "ID", "LineNumber", "TrainID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "ID", "BaseStationID", "CountryID", "Name" },
                values: new object[,]
                {
                    { 2, 1, 1, "Turčin" },
                    { 3, 1, 1, "Doljan" },
                    { 4, 1, 1, "Krušljevec" },
                    { 5, 1, 1, "Čakovec" }
                });

            migrationBuilder.InsertData(
                table: "Stops",
                columns: new[] { "ID", "ArrivalTime", "DepartureTime", "LineID", "Order", "StationID" },
                values: new object[,]
                {
                    { 1, null, new TimeOnly(10, 35, 0), 1, 1, 4 },
                    { 2, new TimeOnly(10, 50, 0), new TimeOnly(10, 55, 0), 1, 2, 3 },
                    { 3, new TimeOnly(11, 10, 0), new TimeOnly(11, 15, 0), 1, 3, 2 },
                    { 4, new TimeOnly(11, 30, 0), null, 1, 4, 1 },
                    { 5, null, new TimeOnly(12, 35, 0), 2, 1, 3 },
                    { 6, new TimeOnly(12, 50, 0), new TimeOnly(12, 55, 0), 2, 2, 1 },
                    { 7, new TimeOnly(13, 10, 0), null, 2, 3, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lines_TrainID",
                table: "Lines",
                column: "TrainID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_BaseStationID",
                table: "Stations",
                column: "BaseStationID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CountryID",
                table: "Stations",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Name",
                table: "Stations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_LineID_Order",
                table: "Stops",
                columns: new[] { "LineID", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_LineID_StationID",
                table: "Stops",
                columns: new[] { "LineID", "StationID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_StationID",
                table: "Stops",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainManufacturer_Name",
                table: "TrainManufacturer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TrainManufacturerID",
                table: "Trains",
                column: "TrainManufacturerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "TrainManufacturer");
        }
    }
}
