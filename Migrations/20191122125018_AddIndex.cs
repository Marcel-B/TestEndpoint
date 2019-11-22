using Microsoft.EntityFrameworkCore.Migrations;

namespace TestPoint.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "DockerImages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RepoName",
                table: "DockerImages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DockerImages_RepoName",
                table: "DockerImages",
                column: "RepoName");

            migrationBuilder.CreateIndex(
                name: "IX_DockerImages_Tag",
                table: "DockerImages",
                column: "Tag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DockerImages_RepoName",
                table: "DockerImages");

            migrationBuilder.DropIndex(
                name: "IX_DockerImages_Tag",
                table: "DockerImages");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "DockerImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RepoName",
                table: "DockerImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
