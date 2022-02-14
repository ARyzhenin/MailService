using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTaskForMonq.Migrations
{
    public partial class ChangeModelToOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs2",
                table: "Logs2");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "Logs2");

            migrationBuilder.RenameTable(
                name: "Logs2",
                newName: "Logs");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "Logs",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMailAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipients_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_LogId",
                table: "Recipients",
                column: "LogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "Logs2");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Logs2",
                newName: "LogId");

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "Logs2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs2",
                table: "Logs2",
                column: "LogId");
        }
    }
}
