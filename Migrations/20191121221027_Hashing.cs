using Microsoft.EntityFrameworkCore.Migrations;

namespace TestPoint.Migrations
{
    public partial class Hashing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeHash",
                table: "DockerImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeHash",
                table: "DockerImages");
        }
    }
}
