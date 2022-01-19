using Microsoft.EntityFrameworkCore.Migrations;

namespace NotifyServer.Migrations;

public class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {

                Id = table.Column<int>(type: "long", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "UserId" },
            values: new object[,]
            {
                { 1, "localid" },
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Users");
    }
}
